using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Helpers;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
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

        

    }
}
