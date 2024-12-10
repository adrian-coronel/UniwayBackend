using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response.PhotoWorkshop;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoWorkshopController : ControllerBase
    {
        private readonly IPhotoWorkshopService _service;
        private readonly ILogger<PhotoWorkshopController> _logger;
        private readonly IMapper _mapper;

        public PhotoWorkshopController(
            IPhotoWorkshopService service,
            ILogger<PhotoWorkshopController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<MessageResponse<PhotoWorkshopResponse>> Save([FromForm] PhotoWorkshopRequest request)
        {
            MessageResponse<PhotoWorkshopResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var photo = await _service.Save(request);

                response = _mapper.Map<MessageResponse<PhotoWorkshop>, MessageResponse<PhotoWorkshopResponse>>(photo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<PhotoWorkshopResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
