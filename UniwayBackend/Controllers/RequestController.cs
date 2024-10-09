using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Request;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

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
        private readonly IStorageService _storageService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _techProfAvaiRequestRepostiory;
        private readonly IMapper _mapper;

        public RequestController(ILogger<RequestController> logger,
                                 IRequestService service,
                                 IImagesProblemRequestService imagesService,
                                 ITechnicalProfessionAvailabilityService techProfAvailabilityService,
                                 IStorageService storageService,
                                 IUserRepository userRepository,
                                 INotificationService notificationService,
                                 ITechnicalProfessionAvailabilityRequestRepository techProfAvaiRequestRepostiory,
                                 IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _imagesService = imagesService;
            _techProfAvailabilityService = techProfAvailabilityService;
            _storageService = storageService;
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

        [HttpPost("RequestByTechnicalProfessionAvailabilityId")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForOne([FromForm] RequestRequest request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Validations
                var validResult = ValidateImages(request.Files);
                if (validResult != null) return validResult;
                
                Request requestEntity = _mapper.Map<Request>(request);

                // Insertar solicitud
                var result = await _service.Save(requestEntity);

                // Guardar imagenes
                if (request.Files.Count > 0)
                {
                    result.Object!.ImagesProblemRequests = await SaveImages(request.Files, result.Object.Id);
                }

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
                            Data = response.Object
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

                // Validations
                var validResult = ValidateImages(request.Files);
                if (validResult != null) return validResult;

                Request requestEntity = _mapper.Map<Request>(request);

                // Buscar TechnicalAvailabilityId
                var techAvai = await _techProfAvailabilityService
                    .GetByTechnicalAndAvailability(request.TechnicalId, request.AvailabilityId);

                if (techAvai.Object == null)
                    return new MessageResponseBuilder<RequestResponse>()
                        .Code(404).Message("No se encontro el técnico con la disponibilidad especificada").Build();
                
                // Asíganamos el tecnico con su disponibilidad a la solicitud
                requestEntity.TechnicalProfessionAvailabilityId = techAvai.Object!.Id;                

                // Insertar solicitud
                var result = await _service.Save(requestEntity);

                // Guardar imagenes
                if (request.Files.Count > 0)
                    result.Object!.ImagesProblemRequests = await SaveImages(request.Files, result.Object.Id);

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
                            Data = response.Object
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

                // Validar imagenes si es que se pasan
                var validResult = ValidateImages(request.Files);
                if (validResult != null) return validResult;

                // Mapear y guardar la entidad de solicitud
                Request requestEntity = _mapper.Map<Request>(request);
                var result = await _service.Save(requestEntity);

                // Guardar imágenes si hay archivos
                if (request.Files.Count > 0)                
                    result.Object!.ImagesProblemRequests = await SaveImages(request.Files, result.Object.Id);
                
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

                    // Enviar notificaciones
                    List<string> ids = nearbyUsers.Select(x => x.Id.ToString()).ToList();
                    await _notificationService.SendSomeNotificationWithRequestAsync(ids, new NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                        Message = "Notification success",
                        Data = response.Object
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
                var result = await _service.Save(requestEntity);

                // Guardar imágenes si hay archivos
                if (request.Files.Count > 0)
                    result.Object!.ImagesProblemRequests = await SaveImages(request.Files, result.Object.Id);

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

                    // Enviar notificaciones
                    List<string> ids = nearbyUsers.Select(x => x.Id.ToString()).ToList();
                    await _notificationService.SendSomeNotificationWithRequestAsync(ids, new NotificationResponse
                    {
                        Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                        Message = "Notification success",
                        Data = response.Object
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





        private async Task<List<ImagesProblemRequest>> SaveImages(List<IFormFile> files, int RequestId)
        {
            var currentDate = DateTime.UtcNow;

            // Guardamos las imagenes 
            List<ImageResponse> images = await _storageService.SaveFilesAsync(files, currentDate.ToString("yyyy-MM-dd"));

            // Guardamos los datos y ubicación de las imgenes en BD
            List<ImagesProblemRequest> imagesProblemMapped = images.Select(x => new ImagesProblemRequest
            {
                RequestId = RequestId,
                Url = x.Url,
                OriginalName = x.OriginalName,
                ExtensionType = x.ExtensionType,
                ContentType = x.ContentType,
                CreatedOn = DateTime.UtcNow,
            }).ToList();
            var imagesProblem = await _imagesService.SaveAll(imagesProblemMapped);

            return imagesProblem.List!.ToList();
        }


        private MessageResponse<RequestResponse>? ValidateImages(List<IFormFile> Files)
        {
            if (Files != null)
            {
                if (Files.Count > Constants.MAX_FILES)
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"La cantidad de archivos excede el limite(${Constants.MAX_FILES})").Build();

                if (Files.Any(x => !Constants.VALID_CONTENT_TYPES.Contains(x.ContentType)))
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"Imagenes con tipo de contenido no valido").Build();

                if (Files.Any(x => x.Length > (Constants.MAX_MB * 1024 * 1024)))
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"Uno de los archivos excedió el tamaño maximo de ${Constants.MAX_MB}MB").Build();
            }
            return null;
        }
    }
}
