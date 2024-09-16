using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.CategoryRequest;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryRequestController : ControllerBase
    {

        private readonly ILogger<CategoryRequestController> _logger;
        private readonly ICategoryRequestService _service;
        private readonly IMapper _mapper;

        public CategoryRequestController(ILogger<CategoryRequestController> logger, ICategoryRequestService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<MessageResponse<CategoryRequestResponse>>> GetAll()
        {
            MessageResponse<CategoryRequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAll();

                response = _mapper.Map<MessageResponse<CategoryRequestResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<CategoryRequestResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
