﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.StateRequest;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class RequestService : IRequestService
    {
        private readonly ILogger<RequestService> _logger;
        private readonly IRequestRepository _repository;
        private readonly IServiceTechnicalRepository _serviceTechnicalRepository;
        private readonly ITechnicalProfessionAvailabilityRepository _technicalProfessionAvailabilityRepository;
        private readonly IImagesProblemRequestService _imagesProblemRequestService;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _techProfAvaiRequestRepository;
        private readonly INotificationService _notification;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly UtilitariesResponse<Request> _utilitaries;
        private readonly UtilitariesResponse<RequestHistoryResponse> _utilitariesHistory;

        public RequestService(ILogger<RequestService> logger,
                              IRequestRepository repository,
                              IImagesProblemRequestService imagesProblemRequestService,
                              IUserRepository userRepository,
                              IServiceTechnicalRepository serviceTechnicalRepository,
                              ITechnicalProfessionAvailabilityRequestRepository techProfAvaiRequestRepository,
                              INotificationService notification,
                              IClientRepository clientRepository,
                              ITechnicalProfessionAvailabilityRepository technicalProfessionAvailabilityRepository,
                              IMapper mapper,
                              UtilitariesResponse<Request> utilitaries,
                              UtilitariesResponse<RequestHistoryResponse> utilitariesHistory)
        {
            _logger = logger;
            _repository = repository;
            _imagesProblemRequestService = imagesProblemRequestService;
            _userRepository = userRepository;
            _serviceTechnicalRepository = serviceTechnicalRepository;
            _techProfAvaiRequestRepository = techProfAvaiRequestRepository;
            _technicalProfessionAvailabilityRepository = technicalProfessionAvailabilityRepository;
            _notification = notification;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _utilitaries = utilitaries;
            _utilitariesHistory = utilitariesHistory;
        }

        public async Task<MessageResponse<Request>> GetAllByUser(Guid userId)
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

        public async Task<MessageResponse<RequestHistoryResponse>> GetAllHistoryByUser(Guid userId)
        {
            MessageResponse<RequestHistoryResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Obteniendo todas las solicitudes pendientes por usuario
                var requests = await _repository.FindAllByUser(userId);

                // Procesando las solicitudes por mes, semana y estado
                var history = requests
                    //.Where(x => x.StateRequestId != Constants.StateRequests.PENDING) // Excluyendo solicitudes pendientes
                    .GroupBy(x => x.CreatedOn.Month) // Agrupando por mes
                    .Select(monthGroup => new RequestHistoryResponse
                    {
                        Month = new DateTime(DateTime.Now.Year, monthGroup.Key, 1).ToString("MMMM"), // Representa el mes como un DateTime
                        StateResponses = monthGroup
                            .GroupBy(x => x.StateRequest) // Agrupando por estado de solicitud
                            .Select(stateGroup => new RequestsForStateResponse
                            {
                                 StateRequest = new StateRequestResponse { Id = stateGroup.Key.Id, Name = stateGroup.Key.Name }, // Creando el estado con su ID
                                 Requests = _mapper.Map<List<RequestResponse>>(stateGroup
                                    .OrderBy(r => r.CreatedOn)) // Mapeando y ordenando las solicitudes por fecha de creación
                            }).ToList()
                    })
                    .OrderByDescending(x => x.Month)
                    .ToList();

                // Generando la respuesta con el historial procesado
                response = _utilitariesHistory.setResponseBaseForList(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitariesHistory.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Request>> GetAllScheduledRequest(int TechnicalProfessionAvailabilityId)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var requests = await _repository.FindAllScheduledRequest(TechnicalProfessionAvailabilityId);

                response = _utilitaries.setResponseBaseForList(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
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

                request.CreatedOn= DateTime.Now;
                if (request.Id != 0) return _utilitaries.setResponseBaseForInternalServerError();

                // Validations
                var validResult = ValidateImages(Files);
                if (validResult != null) return validResult;

                // Insertar solicitud
                //Buscamos el TechnicalProfessionAvailability En caso envieserviceny type car
                if(request.ServiceTechnicalId.HasValue && request.TypeCarId.HasValue)
                {
                    TechnicalProfessionAvailability responseTechnicalProfessionAvailability = await _serviceTechnicalRepository.FindTechnicalProfessionAvailibiltyByServiceId(request.ServiceTechnicalId.Value);
                    request.TechnicalProfessionAvailabilityId = responseTechnicalProfessionAvailability.Id;
                
                }
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

                #region "Notificar a los técnicos que la propuesta ya fue tomada"
                // Si se esta aceptando una propuesta
                //if (requestFind.StateRequestId == Constants.StateRequests.PENDING && // El estado del request esta pendiente
                //    stateRequestId == Constants.StateRequests.IN_PROCESS && // se quiere actualizar en proceso
                //    requestFind.TechnicalProfessionAvailabilityId == null // el tecnico elegido se encuentra en bandeja
                //   )
                //{
                //    var techRequests = await _techProfAvaiRequestRepository.FindAllPendingByRequestId(requestId, Constants.StateRequests.PENDING);

                //    // Notificar a los técnicos que la propuesta ya fue tomada
                //    if (techRequests.Any())
                //    {

                //        // Obtener Ids de los usuarios que fueron rechazados
                //        var techProfAvaIds = techRequests
                //            .Where(x => x.TechnicalProfessionAvailabilityId != technicalProfessionAvailabilityId)
                //            .Select(x => x.TechnicalProfessionAvailabilityId)
                //            .ToList();
                //        var userIdsRechazed = await _userRepository.FindByListTechnicalProfessionAvailabilityId(techProfAvaIds);

                //        if (userIdsRechazed.Any())
                //        {
                //            await _notification.SendSomeNotificationChangeStateRequestAsync(
                //                userIdsRechazed.Select(u => u.Id.ToString()).ToList(),
                //                new Models.Payloads.Core.Response.Notification.NotificationResponse
                //                {
                //                    Type = Constants.TypesConnectionSignalR.RESPONSE,
                //                    Message = $"Solicitud '{shortDescription}' fue tomada",
                //                    Data = null,
                //                    UserSend = new DataUserResponse
                //                    {
                //                        EntityId = requestFind.Client.Id.ToString(),
                //                        FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                //                        PhoneNumber = requestFind.Client.PhoneNumber,
                //                        TypeEntity = Constants.EntityTypes.CLIENT
                //                    }
                //                }
                //            );
                //        }

                //    }
                //    // Asignamos el tecnico/taller a la solicitud
                //    requestFind.TechnicalProfessionAvailabilityId = technicalProfessionAvailabilityId;
                //    // Remover propuestas de la bandeja de pendientes
                //    await _techProfAvaiRequestRepository.DeleteRange(techRequests);
                //}


                //// Actualizar state request
                //requestFind.StateRequestId = stateRequestId;
                //requestFind = await _repository.UpdateAndReturn(requestFind);
                #endregion


                #region "Taller responde solicitud"
                if (requestFind.StateRequestId == Constants.StateRequests.PENDING && // El estado del request esta pendiente
                   stateRequestId == Constants.StateRequests.RESPONDING)// el tecnico elegido se encuentra en bandeja)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(technicalProfessionAvailabilityId);

                    // Asignamos el tecnico/taller a la solicitud
                    requestFind.TechnicalProfessionAvailabilityId = technicalProfessionAvailabilityId;
                    requestFind.StateRequestId = stateRequestId;
                    requestFind = await _repository.UpdateAndReturn(requestFind);

                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    await _notification.SendNotificationChangeStateRequestAsync(
                        user.Id.ToString(),
                        new Models.Payloads.Core.Response.Notification.NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            Message = "RESPUSETA DE TALLER",
                            StateRequestId = requestFind.StateRequestId,
                            TypeAttentionRequest = requestFind.TypeAttention,
                            Data = requestMap,
                            UserSend = new DataUserResponse
                            {
                                EntityId = "0",
                                FullName = $"xd",
                                PhoneNumber = "xd",
                                TypeEntity = Constants.EntityTypes.WORKSHOP
                            }
                        }
                    );
                }

                if (requestFind.StateRequestId == Constants.StateRequests.RESPONDING && // El estado del request esta pendiente
               stateRequestId == Constants.StateRequests.SCHEDULED_ON_HOLD && // se quiere actualizar en proceso
               requestFind.TechnicalProfessionAvailabilityId != null)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(technicalProfessionAvailabilityId);

                    // Asignamos el tecnico/taller a la solicitud
                    requestFind.StateRequestId = stateRequestId;
                    requestFind = await _repository.UpdateAndReturn(requestFind);

                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    await _notification.SendNotificationChangeStateRequestAsync(
                        user.Id.ToString(),
                        new Models.Payloads.Core.Response.Notification.NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            Message = $"La solicitud '{shortDescription}' cambio de estado a {Constants.StateRequests.GetName(stateRequestId)}",
                            StateRequestId = requestFind.StateRequestId,
                            Data = requestMap,
                            TypeAttentionRequest = requestFind.TypeAttention,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestFind.Client.Id.ToString(),
                                FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                                PhoneNumber = requestFind.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
                        }
                    );
                }

                #endregion


                if (requestFind.StateRequestId == Constants.StateRequests.PENDING && // El estado del request esta pendiente
                    stateRequestId == Constants.StateRequests.IN_PROCESS && // se quiere actualizar en proceso
                    requestFind.TechnicalProfessionAvailabilityId == null )// el tecnico elegido se encuentra en bandeja)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(technicalProfessionAvailabilityId);

                    // Asignamos el tecnico/taller a la solicitud
                     requestFind.TechnicalProfessionAvailabilityId = technicalProfessionAvailabilityId;
                     requestFind.StateRequestId = stateRequestId;
                     requestFind = await _repository.UpdateAndReturn(requestFind);

                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    await _notification.SendNotificationChangeStateRequestAsync(
                        user.Id.ToString(),
                        new Models.Payloads.Core.Response.Notification.NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            Message = $"La solicitud '{shortDescription}' cambio de estado a {Constants.StateRequests.GetName(stateRequestId)}",
                            StateRequestId = requestFind.StateRequestId,
                            TypeAttentionRequest = requestFind.TypeAttention ,
                            Data = requestMap,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestFind.Client.Id.ToString(),
                                FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                                PhoneNumber = requestFind.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
                        }
                    );
                }

                if (requestFind.StateRequestId == Constants.StateRequests.PENDING && // El estado del request esta pendiente
                    stateRequestId == Constants.StateRequests.IN_PROCESS && // se quiere actualizar en proceso
                    requestFind.TechnicalProfessionAvailabilityId != null)// el tecnico elegido se encuentra en bandeja)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(technicalProfessionAvailabilityId);

                    // Asignamos el tecnico/taller a la solicitud
                    requestFind.StateRequestId = stateRequestId;
                    requestFind = await _repository.UpdateAndReturn(requestFind);

                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    await _notification.SendNotificationChangeStateRequestAsync(
                        user.Id.ToString(),
                        new Models.Payloads.Core.Response.Notification.NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            Message = $"La solicitud '{shortDescription}' cambio de estado a {Constants.StateRequests.GetName(stateRequestId)}",
                            StateRequestId = requestFind.StateRequestId,
                            Data = requestMap,
                            TypeAttentionRequest = requestFind.TypeAttention,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestFind.Client.Id.ToString(),
                                FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                                PhoneNumber = requestFind.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
                        }
                    );
                }


                if (requestFind.StateRequestId == Constants.StateRequests.CLOSURE_REQUEST && // El estado del request esta en solicitu de cierre
                    stateRequestId == Constants.StateRequests.CLOSED && // se quiere actualizar a completado
                    requestFind.TechnicalProfessionAvailabilityId != null)// el tecnico elegido se encuentra en bandeja)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(technicalProfessionAvailabilityId);
                    var technical = await _technicalProfessionAvailabilityRepository.FindTechnicalInformationByTechnicalProfeessionAvailabilityId(technicalProfessionAvailabilityId);
                    requestFind.StateRequestId = stateRequestId;
                    requestFind = await _repository.UpdateAndReturn(requestFind);

                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    await _notification.SendNotificationChangeStateRequestAsync(
                        user.Id.ToString(),
                        new Models.Payloads.Core.Response.Notification.NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            Message = $"El cliente ha aceptado el cierre del servicio!",
                            Data = requestMap,
                            StateRequestId = stateRequestId,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestFind.Client.Id.ToString(),
                                FullName = $"{requestFind.Client.Name} {requestFind.Client.FatherLastname} {requestFind.Client.MotherLastname}",
                                PhoneNumber = requestFind.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT,
                                TechnicalId = technical.TechnicalProfession.UserTechnical.TechnicalId.ToString()
                            }
                        }
                    );
                }

                if (stateRequestId == Constants.StateRequests.CLOSURE_REQUEST)
                {
                    // Enviar la notificacion al cliente
                    User? user = await _userRepository.FindByRequestId(requestFind.Id);
                    DataUserResponse userSend = await _userRepository.FindTechnicalOrWorkshop(requestFind.TechnicalProfessionAvailabilityId.Value);
                    
                    var requestMap = _mapper.Map<RequestResponse>(requestFind);
                    requestFind.StateRequestId = stateRequestId;
                    requestFind = await _repository.UpdateAndReturn(requestFind);
                    var technical = await _technicalProfessionAvailabilityRepository.FindById(requestFind.TechnicalProfessionAvailabilityId.Value);
                    await _notification.SendNotificationChangeStateRequestAsync(user!.Id.ToString(), new Models.Payloads.Core.Response.Notification.NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.CLOSE_SOLICITUDE,
                        Message = $"Solicitud de cierre del servicio",
                        Data = requestMap,
                        UserSend = new DataUserResponse
                        {
                            EntityId = technical.Id.ToString(),
                            FullName = $"xd",
                            PhoneNumber = "xd",
                            TypeEntity = Constants.EntityTypes.MECHANICAL

                        }
                    });
                }

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
