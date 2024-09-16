using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Models.Payloads.Core.Response.StateRequest;
using System.Reflection;

namespace UniwayBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateRequestController : ControllerBase
    {
        private readonly ILogger<StateRequestController> _logger;
        private readonly IStateRequestService _service;
        private readonly IMapper _mapper;

        public StateRequestController(ILogger<StateRequestController> logger, IStateRequestService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<MessageResponse<StateRequestResponse>>> GetAll()
        {
            MessageResponse<StateRequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAll();

                response = _mapper.Map<MessageResponse<StateRequestResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<StateRequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
