using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Config;
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
        private readonly IMapper _mapper;

        public RequestController(ILogger<RequestController> logger,
                                 IRequestService service,
                                 IImagesProblemRequestService imagesService,
                                 ITechnicalProfessionAvailabilityService techProfAvailabilityService,
                                 IStorageService storageService,
                                 IUserRepository userRepository,
                                 INotificationService notificationService,
                                 IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _imagesService = imagesService;
            _techProfAvailabilityService = techProfAvailabilityService;
            _storageService = storageService;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        [HttpPost("SaveRequestForOne")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForOne([FromForm] RequestRequest request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Validations
                var validResult = ValidateRequest(request);
                if (validResult != null) return validResult;
                
                // Buscar TechnicalAvailabilityId
                if (request.TechnicalId != null && request.AvailabilityId != null && request.TechnicalProfessionAvailabilityId == null)
                {
                    var techAvai = await _techProfAvailabilityService
                        .GetByTechnicalAndAvailability(request.TechnicalId.Value ,request.AvailabilityId.Value);
                    
                    if (techAvai.Object != null) request.TechnicalProfessionAvailabilityId = techAvai.Object!.Id;
                }

                Request requestEntity = _mapper.Map<Request>(request);

                // Insertar solicitud
                var result = await _service.Save(requestEntity);

                if (request.Files.Count > 0)
                {
                    result.Object!.ImagesProblemRequests = await SaveImages(request, result.Object.Id);
                }


                response = _mapper.Map<MessageResponse<RequestResponse>>(result);
                
                // Notificar a un usuario
                if (response.Object!.TechnicalProfessionAvailabilityId != null)
                {
                    var user = await _userRepository.FindByTechnicalProfessionAvailabilityId(response.Object!.TechnicalProfessionAvailabilityId.Value);

                    if (user != null)
                        await _notificationService.SendNotificationAsync(user.Id.ToString(), new NotificationResponse
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
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequestForMany([FromForm] RequestRequest request)
        {
            MessageResponse<RequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var validResult = ValidateRequest(request);
                if (validResult != null) return validResult;

                Request requestEntity = _mapper.Map<Request>(request);

                // Insertar solicitud
                var result = await _service.Save(requestEntity);

                if (request.Files.Count > 0)
                {
                    result.Object!.ImagesProblemRequests = await SaveImages(request, result.Object.Id);
                }

                response = _mapper.Map<MessageResponse<RequestResponse>>(result);
            }
            catch (Exception ex)
            {
                response = new MessageResponseBuilder<RequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }


        private async Task<List<ImagesProblemRequest>> SaveImages(RequestRequest request, int RequestId)
        {
            var currentDate = DateTime.UtcNow;

            List<ImageResponse> images = await _storageService.SaveFilesAsync(request.Files, currentDate.ToString("yyyy-MM-dd"));

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


        private MessageResponse<RequestResponse>? ValidateRequest(RequestRequest request)
        {
            if (request.Files != null)
            {
                if (request.Files.Count > Constants.MAX_FILES)
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"La cantidad de archivos excede el limite(${Constants.MAX_FILES})").Build();

                if (request.Files.Any(x => !Constants.VALID_CONTENT_TYPES.Contains(x.ContentType)))
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"Imagenes con tipo de contenido no valido").Build();

                if (request.Files.Any(x => x.Length > (Constants.MAX_MB * 1024 * 1024)))
                    return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"Uno de los archivos excedió el tamaño maximo de ${Constants.MAX_MB}MB").Build();
            }
            // Se debe pasar TechnicalProfessionAvailabilityId o TechnicalId y AvailabilityId
            if (request.TechnicalProfessionAvailabilityId == null && (request.TechnicalId == null || request.AvailabilityId == null))
            {
                return new MessageResponseBuilder<RequestResponse>()
                    .Code(400).Message($"Debe ingresar TechnicalProfessionAvailabilityId o TechnicalId y AvailabilityId").Build();
            }

            return null;
        }
    }
}
