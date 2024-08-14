using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Dtos;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopTechnicalProfessionController : ControllerBase
    {

        private readonly IWorkshopTechnicalProfessionService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkshopTechnicalProfessionController> _logger;

        public WorkshopTechnicalProfessionController(IWorkshopTechnicalProfessionService service, IMapper mapper, ILogger<WorkshopTechnicalProfessionController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse<WorkshopTechnicalProfessionDto>>> Save([FromBody] WorkshopTechnicalProfessionRequest Request)
        {
            MessageResponse<WorkshopTechnicalProfessionDto> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var request = _mapper.Map<WorkshopTechnicalProfessionRequest, WorkshopTechnicalProfession>(Request);

                var result = await _service.Save(request);

                response = _mapper.Map<MessageResponse<WorkshopTechnicalProfession>, MessageResponse<WorkshopTechnicalProfessionDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<WorkshopTechnicalProfessionDto>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);

        }
    }
}
