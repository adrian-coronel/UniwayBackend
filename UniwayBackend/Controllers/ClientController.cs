using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Client;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientService _service;

        public ClientController(ILogger<ClientController> logger, IMapper mapper, IClientService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<ActionResult<MessageResponse<ClientResponseV2>>> GetById(int Id)
        {
            MessageResponse<ClientResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetById(Id);

                response = _mapper.Map<MessageResponse<ClientResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ClientResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetByUserId/{UserId}")]
        public async Task<ActionResult<MessageResponse<ClientResponseV2>>> GetById(Guid UserId)
        {
            MessageResponse<ClientResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetInformationByUser(UserId);

                response = _mapper.Map<MessageResponse<ClientResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ClientResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }
    }
}
