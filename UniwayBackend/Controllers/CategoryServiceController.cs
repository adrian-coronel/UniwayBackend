using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.CategoryService;
using UniwayBackend.Models.Payloads.Core.Response.CategoryService;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryServiceController : ControllerBase
    {
        private readonly ILogger<CategoryServiceController> _logger;
        private readonly ICategoryServiceService _service;
        private readonly IMapper _mapper;

        public CategoryServiceController(ILogger<CategoryServiceController> logger, ICategoryServiceService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("TechnicalProfessionAvailability/{TechnicalProfessionAvailabilityId}")]
        public async Task<ActionResult<MessageResponse<CategoryServiceResponse>>> GetByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId)
        {
            MessageResponse<CategoryServiceResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllByTechnicalProfessionAvailabilityId(TechnicalProfessionAvailabilityId);

                response = _mapper.Map<MessageResponse<CategoryServiceResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<CategoryServiceResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPost("GetServicesOneCategory")]
        public async Task<ActionResult<MessageResponse<CategoryServiceResponse>>> GetServicesOneCategory([FromBody] CategoryServiceRequest request)
        {
            MessageResponse<CategoryServiceResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllByIdAndTechnicalProfessionAvailabilityId(request.Id, request.TechnicalProfessionAvailabilityId); ;

                response = _mapper.Map<MessageResponse<CategoryServiceResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<CategoryServiceResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
