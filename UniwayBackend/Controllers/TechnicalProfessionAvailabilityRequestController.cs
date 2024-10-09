using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalProfessionAvailabilityRequestController : ControllerBase
    {
        private readonly ITechnicalProfessionAvailabilityRequestService _service;
        private readonly ILogger<TechnicalProfessionAvailabilityRequestController> _logger;
        private readonly IMapper _mapper;

        public TechnicalProfessionAvailabilityRequestController(ITechnicalProfessionAvailabilityRequestService service,
                                                                ILogger<TechnicalProfessionAvailabilityRequestController> logger,
                                                                IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet("GetGroupRequestPending/{UserId}")]
        public async Task<ActionResult<MessageResponse<TechnicalProfessionAvailabilityRequestResponseV2>>> GetGroupRequestPending(Guid UserId)
        {
            MessageResponse<TechnicalProfessionAvailabilityRequestResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllPendingByUserId(UserId);

                response = _mapper.Map<MessageResponse<TechnicalProfessionAvailabilityRequestResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<TechnicalProfessionAvailabilityRequestResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }
    }
}
