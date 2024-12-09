using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.PhotoUser;
using UniwayBackend.Models.Payloads.Core.Response.PhotoUser;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoUserController : ControllerBase
    {
        private readonly IPhotoUserService _service;
        private readonly ILogger<PhotoUserController> _logger;
        private readonly IMapper _mapper;

        public PhotoUserController(
            IPhotoUserService service,
            ILogger<PhotoUserController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<MessageResponse<PhotoUserResponse>> Save([FromForm] PhotoUserRequest request)
        {
            MessageResponse<PhotoUserResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var photo = await _service.Save(request);

                response = _mapper.Map<MessageResponse<PhotoUser>, MessageResponse<PhotoUserResponse>>(photo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<PhotoUserResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
