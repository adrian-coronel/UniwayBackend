using AutoMapper;
using Newtonsoft.Json;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class RequestService : IRequestService
    {
        private readonly ILogger<RequestService> _logger;
        private readonly IRequestRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _techProfAvaiRequestRepository;
        private readonly INotificationService _notification;
        private readonly IMapper _mapper;
        private readonly UtilitariesResponse<Request> _utilitaries;

        public RequestService(ILogger<RequestService> logger,
                              IRequestRepository repository,
                              IUserRepository userRepository,
                              ITechnicalProfessionAvailabilityRequestRepository techProfAvaiRequestRepository,
                              INotificationService notification,
                              IMapper mapper,
                              UtilitariesResponse<Request> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _userRepository = userRepository;
            _techProfAvaiRequestRepository = techProfAvaiRequestRepository;
            _notification = notification;
            _mapper = mapper;
            _utilitaries = utilitaries;
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

        public async Task<MessageResponse<Request>> Save(Request request)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (request.Id != 0) return _utilitaries.setResponseBaseForInternalServerError();

                Request requestSaved = await _repository.InsertAndReturn(request);

                response = _utilitaries.setResponseBaseForObject(requestSaved);
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
                            await _notification.SendSomeNotificationChangeStateRequestAsync(
                                userIdsRechazed.Select(u => u.Id.ToString()).ToList(),
                                new Models.Payloads.Core.Response.Notification.NotificationResponse
                                {
                                    Type = Constants.TypesConnectionSignalR.RESPONSE,
                                    Message = $"Solicitud '{shortDescription}' fue tomada",
                                    Data = null
                                }
                            );

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
                        Data = requestMap
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
    }
}
