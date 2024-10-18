using AutoMapper;
using Newtonsoft.Json;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class RequestService : IRequestService
    {
        private readonly ILogger<RequestService> _logger;
        private readonly IRequestRepository _repository;
        private readonly IImagesProblemRequestService _imagesProblemRequestService;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _techProfAvaiRequestRepository;
        private readonly INotificationService _notification;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly UtilitariesResponse<Request> _utilitaries;

        public RequestService(ILogger<RequestService> logger, IRequestRepository repository, IImagesProblemRequestService imagesProblemRequestService, IUserRepository userRepository, ITechnicalProfessionAvailabilityRequestRepository techProfAvaiRequestRepository, INotificationService notification, IClientRepository clientRepository, IMapper mapper, UtilitariesResponse<Request> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _imagesProblemRequestService = imagesProblemRequestService;
            _userRepository = userRepository;
            _techProfAvaiRequestRepository = techProfAvaiRequestRepository;
            _notification = notification;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Request>> GetRequestPendingForClientAndStateRequest(int clientId, short? StateRequestId)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var requests = await _repository.FindAllPendingByClientIdAndStateRequestId(clientId, StateRequestId ?? 0);

                response = _utilitaries.setResponseBaseForList(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Request>> GetRequestPendingInTrayByUserId(Guid userId)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var requests = await _repository.FindAllPendingByUserId(userId);

                response = _utilitaries.setResponseBaseForList(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Request>> Save(Request request,List<IFormFile> Files)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (request.Id != 0) return _utilitaries.setResponseBaseForInternalServerError();

                // Validations
                var validResult = ValidateImages(Files);
                if (validResult != null) return validResult;

                // Insertar solicitud
                request = await _repository.InsertAndReturn(request);

                // Guardar imagenes
                if (Files.Count > 0)
                {
                    var imgResponse = await _imagesProblemRequestService.SaveAll(request.Id, Files);
                    request.ImagesProblemRequests = imgResponse.List!.ToList();
                }

                request.Client = await _clientRepository.FindById(request.ClientId);

                response = _utilitaries.setResponseBaseForObject(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Request>> UpdateStateRequestByRequestId(int requestId, short stateRequestId, int technicalProfessionAvailabilityId = 0)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var requestFind = await _repository.FindById(requestId);

                requestFind.Client = await _clientRepository.FindById(requestFind.ClientId);

                // Preparamos el mensaje
                string shortDescription = requestFind.Description.Length > 20
                            ? requestFind.Description.Substring(0, 20) + "..."
                            : requestFind.Description;

                // Si se esta aceptando una propuesta
                if (requestFind.StateRequestId == Constants.StateRequests.PENDING && // El estado del request esta pendiente
                    stateRequestId == Constants.StateRequests.IN_PROCESS && // se quiere actualizar en proceso
                    requestFind.TechnicalProfessionAvailabilityId == null // el tecnico elegido se encuentra en bandeja
                   )
                {
                    var techRequests = await _techProfAvaiRequestRepository.FindAllPendingByRequestId(requestId, Constants.StateRequests.PENDING);

                    // Notificar a los técnicos que la propuesta ya fue tomada
                    if (techRequests.Any())
                    {

                        // Obtener Ids de los usuarios que fueron rechazados
                        var techProfAvaIds = techRequests
                            .Where(x => x.TechnicalProfessionAvailabilityId != technicalProfessionAvailabilityId)
                            .Select(x => x.TechnicalProfessionAvailabilityId)
                            .ToList();
                        var userIdsRechazed = await _userRepository.FindByListTechnicalProfessionAvailabilityId(techProfAvaIds);
                        
                        if (userIdsRechazed.Any())
                        {
                            await _notification.SendSomeNotificationChangeStateRequestAsync(
                                userIdsRechazed.Select(u => u.Id.ToString()).ToList(),
                                new Models.Payloads.Core.Response.Notification.NotificationResponse
                                {
                                    Type = Constants.TypesConnectionSignalR.RESPONSE,
                                    Message = $"Solicitud '{shortDescription}' fue tomada",
                                    Data = null,
                                    UserSend = new DataUserResponse
                                    {
                                        EntityId = requestFind.Client.Id.ToString(),
                                        FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                                        TypeEntity = Constants.EntityTypes.CLIENT
                                    }
                                }
                            );
                        }

                    }
                    // Asignamos el tecnico/taller a la solicitud
                    requestFind.TechnicalProfessionAvailabilityId = technicalProfessionAvailabilityId;
                    // Remover propuestas de la bandeja de pendientes
                    await _techProfAvaiRequestRepository.DeleteRange(techRequests);
                }


                // Actualizar state request
                requestFind.StateRequestId = stateRequestId;
                requestFind = await _repository.UpdateAndReturn(requestFind);

                // Notificar al tecnico elegido 
                var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(requestFind.TechnicalProfessionAvailabilityId.Value);
                var requestMap = _mapper.Map<RequestResponse>(requestFind);
                await _notification.SendNotificationChangeStateRequestAsync(
                    user.Id.ToString(),
                    new Models.Payloads.Core.Response.Notification.NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.RESPONSE,
                        Message = $"La solicitud '{shortDescription}' cambio de estado a {Constants.StateRequests.GetName(stateRequestId)}",
                        Data = requestMap,
                        UserSend = new DataUserResponse
                        {
                            EntityId = requestFind.Client.Id.ToString(),
                            FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                            TypeEntity = Constants.EntityTypes.CLIENT
                        }
                    }
                );

                response = _utilitaries.setResponseBaseForObject(requestFind);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        private MessageResponse<Request>? ValidateImages(List<IFormFile> Files)
        {
            if (Files != null)
            {
                if (Files.Count > Constants.MAX_FILES)
                    return new MessageResponseBuilder<Request>()
                    .Code(400).Message($"La cantidad de archivos excede el limite(${Constants.MAX_FILES})").Build();

                if (Files.Any(x => !Constants.VALID_CONTENT_TYPES.Contains(x.ContentType)))
                    return new MessageResponseBuilder<Request>()
                    .Code(400).Message($"Imagenes con tipo de contenido no valido").Build();

                if (Files.Any(x => x.Length > (Constants.MAX_MB * 1024 * 1024)))
                    return new MessageResponseBuilder<Request>()
                    .Code(400).Message($"Uno de los archivos excedió el tamaño maximo de ${Constants.MAX_MB}MB").Build();
            }
            return null;
        }
    }
}
