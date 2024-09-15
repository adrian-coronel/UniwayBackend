using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Review;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService service, IMapper mapper, ILogger<ReviewController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet("GetAllByTechnical/{TechnicalId}")]
        public async Task<ActionResult<MessageResponse<Review>>> GetAllByTechnical(int TechnicalId)
        {
            MessageResponse<ReviewResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllByTechnical(TechnicalId);

                response = _mapper.Map<MessageResponse<Review>, MessageResponse<ReviewResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ReviewResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet("GetSummaryByTechnical/{TechnicalId}")]
        public async Task<ActionResult<MessageResponse<ReviewSummaryResponse>>> GetSummaryByTechnical(int TechnicalId)
        {
            MessageResponse<ReviewSummaryResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetSummaryByTechnical(TechnicalId);

                response = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ReviewSummaryResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }
    }
}
