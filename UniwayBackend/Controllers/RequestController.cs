using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.StateRequest;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly ILogger<RequestController> _logger;
        private readonly IRequestService _service;
        private readonly IImagesProblemRequestService _imagesService;
        private readonly ITechnicalProfessionAvailabilityService _techProfAvailabilityService;
        //private readonly IStorageService _storageService;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _techProfAvaiRequestRepostiory;
        private readonly IMapper _mapper;

        public RequestController(ILogger<RequestController> logger,
                                 IRequestService service,
                                 IImagesProblemRequestService imagesService,
                                 ITechnicalProfessionAvailabilityService techProfAvailabilityService,
                                 //IStorageService storageService,
                                 IUserRepository userRepository,
                                 INotificationService notificationService,
                                 ITechnicalProfessionAvailabilityRequestRepository techProfAvaiRequestRepostiory,
                                 IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _imagesService = imagesService;
            _techProfAvailabilityService = techProfAvailabilityService;
            //_storageService = storageService;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _techProfAvaiRequestRepostiory = techProfAvaiRequestRepostiory;
            _mapper = mapper;
        }

        [HttpGet("GetRequestPending/{UserId}")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> GetRequestPending(Guid UserId)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetRequestPendingInTrayByUserId(UserId);

                response = _mapper.Map<MessageResponse<RequestResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetScheduledRequest/{TechnicalProfessionAvailabilityId}")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> GetScheduledRequest(int TechnicalProfessionAvailabilityId)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllScheduledRequest(TechnicalProfessionAvailabilityId);

                response = _mapper.Map<MessageResponse<RequestResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetHistoryRequests/{UserId}")]
        public async Task<ActionResult<MessageResponse<RequestHistoryResponse>>> GetHistoryRequests(Guid UserId)
        {
            MessageResponse<RequestHistoryResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                response = await _service.GetAllHistoryByUser(UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RequestHistoryResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetRequestByClientAndStateRequest/{ClientId}/{StateRequestId}")]
        public async Task<ActionResult<MessageResponse<RequestResponseV3>>> GetRequestPending(int ClientId, short StateRequestId)
        {
            MessageResponse<RequestResponseV3> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetRequestPendingForClientAndStateRequest(ClientId, StateRequestId);

                response = _mapper.Map<MessageResponse<RequestResponseV3>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RequestResponseV3>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPost("RequestByTechnicalProfessionAvailabilityId")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForOne([FromForm] RequestRequest request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                Request requestEntity = _mapper.Map<Request>(request);

                // Insertar solicitud
                var result = await _service.Save(requestEntity, request.Files);

                // Mapeamos la solicitud a un tipo respuesta
                response = _mapper.Map<MessageResponse<RequestResponse>>(result);

                // Notificar a un usuario
                if (response.Object!.TechnicalProfessionAvailabilityId != null && response.Code == 200)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(response.Object!.TechnicalProfessionAvailabilityId.Value);

                    if (user != null)
                        await _notificationService.SendNotificationWithRequestAsync(user.Id.ToString(), new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                            Message = "Notification success",
                            Data = response.Object,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestEntity.Id.ToString(),
                                FullName = $"{requestEntity.Client.Name} {requestEntity.Client.FatherLastname} {requestEntity.Client.MotherLastname}",
                                PhoneNumber = requestEntity.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
                        });
                }

            }
            catch (Exception ex)
            {
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost("RequestByTechnicalIdAndAvailabilityId")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForOne2([FromForm] RequestRequestV3 request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Buscar TechnicalAvailabilityId
                var techAvai = await _techProfAvailabilityService
                    .GetByTechnicalAndAvailability(request.TechnicalId, request.AvailabilityId);

                if (techAvai.Object == null)
                    return new MessageResponseBuilder<RequestResponse>()
                        .Code(404).Message("No se encontro el técnico con la disponibilidad especificada").Build();


                Request requestEntity = _mapper.Map<Request>(request);

                // Asíganamos el tecnico con su disponibilidad a la solicitud
                requestEntity.TechnicalProfessionAvailabilityId = techAvai.Object!.Id;                

                // Insertar solicitud
                var result = await _service.Save(requestEntity, request.Files);

                // Mapeamos la solicitud a un tipo respuesta
                response = _mapper.Map<MessageResponse<RequestResponse>>(result);

                // Notificar a un usuario
                if (response.Object!.TechnicalProfessionAvailabilityId != null && response.Code == 200)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(response.Object!.TechnicalProfessionAvailabilityId.Value);
                    var userSend = await _userRepository.FindByClientId(request.ClientId);
                    if (user != null)
                        await _notificationService.SendNotificationWithRequestAsync(user.Id.ToString(), new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                            Message = "Notification success",
                            Data = response.Object,
                            UserSend = new DataUserResponse
                            {
                                EntityId = requestEntity.Id.ToString(),
                                FullName = $"{requestEntity.Client.Name} {requestEntity.Client.FatherLastname} {requestEntity.Client.MotherLastname}",
                                PhoneNumber  = requestEntity.Client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
                        });
                }

            }
            catch (Exception ex)
            {
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }


        // SE BUSCA LOS CERCANOS EN EL BACKEND
        [HttpPost("SaveRequestForMany")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForMany([FromForm] RequestManyRequestV4 request)
        {
            MessageResponse<RequestResponse> response;

            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Mapear y guardar la entidad de solicitud
                Request requestEntity = _mapper.Map<Request>(request);

                if (requestEntity.TypeCarId == 0) requestEntity.TypeCarId = null;

                var result = await _service.Save(requestEntity, request.Files);
              
                // Mapeamos la solicitud a un tipo respuesta
                response = _mapper.Map<MessageResponse<RequestResponse>>(result);

                // Buscar tecnicos disponibles
                var referenceLocation = new Point(request.Lng, request.Lat) { SRID = 4326 };
                var techAvailabilities = await _techProfAvailabilityService.GetByAvailabilityAndLocation(referenceLocation, request.AvailabilityId, request.Distance);

                // Si se encontraron técnicos disponibles
                if (techAvailabilities.List != null && techAvailabilities.List.Any() && response.Code == 200)
                {
                    // Guardar la relación de la solicitud a técnicos cercanos
                    var requestToTech = techAvailabilities.List.Select(x => new TechnicalProfessionAvailabilityRequest
                    {
                        TechnicalProfessionAvailabilityId = x.Id,
                        RequestId = result.Object!.Id
                    }).ToList();
                    await _techProfAvaiRequestRepostiory.InsertAll(requestToTech);                    

                    // Obtener el Id de los técnicos relacionados para notificar
                    List<int> IdsTechProfAvai = techAvailabilities.List.Select(x => x.Id).ToList();
                    List<User> nearbyUsers = await _userRepository.FindByListTechnicalProfessionAvailabilityId(IdsTechProfAvai);
                    var userSend = await _userRepository.FindByClientId(request.ClientId);
                    // Enviar notificaciones
                    List<string> ids = nearbyUsers.Select(x => x.Id.ToString()).ToList();
                    await _notificationService.SendSomeNotificationWithRequestAsync(ids, new NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                        Message = "Notification success",
                        Data = response.Object,
                        TypeAttentionRequest=request.TypeAttention,
                        UserSend = new DataUserResponse
                        {
                            EntityId = requestEntity.Id.ToString(),
                            FullName = $"{requestEntity.Client.Name} {requestEntity.Client.FatherLastname} {requestEntity.Client.MotherLastname}",
                            PhoneNumber = requestEntity.Client.PhoneNumber,
                            TypeEntity = Constants.EntityTypes.CLIENT
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }

            return StatusCode(response.Code, response);
        }


        [HttpPost("SaveScheduleRequest")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveScheduleRequest([FromForm] RequestManyRequestV5 request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Mapear y guardar la entidad de solicitud
                Request requestEntity = _mapper.Map<Request>(request);

                var result = await _service.Save(requestEntity, request.Files);

                // Mapeamos la solicitud a un tipo respuesta
                response = _mapper.Map<MessageResponse<RequestResponse>>(result);

                // Buscar tecnicos/talleres disponibles
                var referenceLocation = new Point(request.Lng, request.Lat) { SRID = 4326 };
                var techAvailabilities = await _techProfAvailabilityService.GetByAvailabilityAndLocation(referenceLocation, request.AvailabilityId, request.Distance);

                // Si se encontraron técnicos/talleres disponibles
                if (techAvailabilities.List != null && techAvailabilities.List.Any() && response.Code == 200)
                {
                    // Guardar la relación de la solicitud a técnicos cercanos
                    var requestToTech = techAvailabilities.List.Select(x => new TechnicalProfessionAvailabilityRequest
                    {
                        TechnicalProfessionAvailabilityId = x.Id,
                        RequestId = result.Object!.Id
                    }).ToList();
                    await _techProfAvaiRequestRepostiory.InsertAll(requestToTech);

                    // Obtener el Id de los técnicos relacionados para notificar
                    List<int> IdsTechProfAvai = techAvailabilities.List.Select(x => x.Id).ToList();
                    List<User> nearbyUsers = await _userRepository.FindByListTechnicalProfessionAvailabilityId(IdsTechProfAvai);
                    var userSend = await _userRepository.FindByClientId(request.ClientId);

                    // Enviar notificaciones
                    List<string> ids = nearbyUsers.Select(x => x.Id.ToString()).ToList();
                    await _notificationService.SendSomeNotificationWithRequestAsync(ids, new NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                        Message = "Notification success",
                        Data = response.Object,
                        UserSend = new DataUserResponse
                        {
                            EntityId = requestEntity.Id.ToString(),
                            FullName = $"{requestEntity.Client.Name} {requestEntity.Client.FatherLastname} {requestEntity.Client.MotherLastname}",
                            PhoneNumber = requestEntity.Client.PhoneNumber,
                            TypeEntity = Constants.EntityTypes.CLIENT
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// TechnicalProfessionAvailabilityId se deberá pasar como null cuando la solicitud tenga un tecnico/taller especificado
        /// TechnicalProfessionAvailabilityId se no se deberá pasar como null cuando el cliente esta aceptando la respuesta de un tecnico/taller
        [HttpPut("UpdateStateRequest")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> UpdateStateRequest([FromBody] StateRequestRequestV1 request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.UpdateStateRequestByRequestId(
                    request.RequestId, 
                    request.StateRequestId, 
                    request.TechnicalProfessionAvailabilityId ?? 0);

                response = _mapper.Map<MessageResponse<RequestResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }


       
    }
}
