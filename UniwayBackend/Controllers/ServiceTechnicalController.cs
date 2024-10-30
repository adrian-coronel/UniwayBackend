using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Request.ServiceTechnical;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

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

        [HttpGet("GetById/{Id}")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalResponse>>> GetById(int Id)
        {
            MessageResponse<ServiceTechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetById(Id);

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

        [HttpGet("{TechnicalId}/{AvailabilityId}")]
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


        [HttpPost("Save")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalResponse>>> Save([FromForm] ServiceTechnicalRequest request)
        {
            MessageResponse<ServiceTechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var imageServiceTechnical = _mapper.Map<ServiceTechnical>(request);

                var result = await _service.Save(imageServiceTechnical, request.Files);

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

        [HttpPut("Update")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalResponse>>> Update([FromBody] ServiceTechnicalRequestV2 request)
        {
            MessageResponse<ServiceTechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var ServiceTechnical = _mapper.Map<ServiceTechnical>(request);

                var result = await _service.Update(ServiceTechnical);

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
