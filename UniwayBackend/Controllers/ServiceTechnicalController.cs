using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTechnicalController : ControllerBase
    {

        private readonly IServiceTechnicalService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceTechnicalController> _logger;

        public ServiceTechnicalController(IServiceTechnicalService service, IMapper mapper, ILogger<ServiceTechnicalController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet("/{TechnicalId}/{AvailabilityId}")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalResponse>>> GetTechnicalAndAvailability(int TechnicalId, short AvailabilityId)
        {
            MessageResponse<ServiceTechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetByTechnicaIdAndAvailabilityId(TechnicalId, AvailabilityId);

                response = _mapper.Map<MessageResponse<ServiceTechnicalResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ServiceTechnicalResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }


    }
}
