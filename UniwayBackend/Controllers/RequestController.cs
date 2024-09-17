using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Request;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
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
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public RequestController(ILogger<RequestController> logger, IRequestService service, IImagesProblemRequestService imagesService, IStorageService storageService, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _imagesService = imagesService;
            _storageService = storageService;
            _mapper = mapper;
        }

        [HttpPost("SaveRequest")]
        public async Task<ActionResult<MessageResponse<RequestResponse>>> SaveRequest([FromForm] RequestRequest request)
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
                    var currentDate = DateTime.UtcNow;

                    List<ImageResponse> images = await _storageService.SaveFilesAsync(request.Files,currentDate.ToString("yyyy-mm-dd"));

                    List<ImagesProblemRequest> imagesProblemMapped = images.Select(x => new ImagesProblemRequest
                    {
                        RequestId = result.Object!.Id,
                        Url = x.Url,
                        OriginalName = x.OriginalName,
                        ExtensionType = x.ExtensionType,
                        ContentType = x.ContentType,
                        FakeName = x.FakeName,
                        FakeExtensionType = x.FakeExtensionType,
                        CreatedOn = DateTime.UtcNow,
                    }).ToList();

                    var imagesProblem = await _imagesService.SaveAll(imagesProblemMapped);

                    result.Object!.ImagesProblemRequests = imagesProblem.List!.ToList();
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

            return null;
        }
    }
}
