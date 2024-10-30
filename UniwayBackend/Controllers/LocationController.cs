using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Location;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Location;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LocationController : ControllerBase
    {

        private readonly ILocationService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationService service, IMapper mapper, ILogger<LocationController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("GetAllByAvailability")]
        public async Task<ActionResult<MessageResponse<LocationResponse>>> GetAll(LocationRequest request)
        {
            MessageResponse<LocationResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                
                response = await _service.GetAllByAvailability(request);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<LocationResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPut("UpdateByTechnicalProfessionAvailability")]
        public async Task<ActionResult<MessageResponse<LocationResponse>>> UpdateByTechnicalProfessionAvailability(LocationRequestV2 request)
        {
            MessageResponse<LocationResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                response = await _service.UpdateByTechnicalProfessionAvailability(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<LocationResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost("GetAllByAvailabilityWithServices")]
        public async Task<ActionResult<MessageResponse<LocationResponseV2>>> GetAllWithServices(LocationRequest request)
        {
            MessageResponse<LocationResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                response = await _service.GetAllByAvailabilityWithServices(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<LocationResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

    }
}
