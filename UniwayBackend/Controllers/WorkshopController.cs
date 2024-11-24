using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Workshop;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {

        private readonly IWorkshopService _service;
        private readonly ILogger<WorkshopController> _logger;
        private readonly IMapper _mapper;

        public WorkshopController(IWorkshopService service, ILogger<WorkshopController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPut("UpdateWorkingStatus")]
        public async Task<ActionResult<MessageResponse<WorkshopResponse>>> UpdateWorkingStatus([FromBody] WorkshopRequestV1 request)
        {
            MessageResponse<WorkshopResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.UpdateWorkshopStatus(request);

                response = _mapper.Map<MessageResponse<WorkshopResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<WorkshopResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPost("Save")]
        public async Task<ActionResult<MessageResponse<WorkshopResponse>>> Save([FromBody] WorkshopRequestV2 request)
        {
            MessageResponse<WorkshopResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var user = User.FindFirst("Id")?.Value;

                var workshop = _mapper.Map<Workshop>(request);

                var result = await _service.Save(workshop);

                response = _mapper.Map<MessageResponse<WorkshopResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<WorkshopResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPut("Update")]
        public async Task<ActionResult<MessageResponse<WorkshopResponse>>> Update([FromBody] WorkshopRequestV2 request)
        {
            MessageResponse<WorkshopResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var workshop = _mapper.Map<Workshop>(request);

                var result = await _service.Update(workshop);

                response = _mapper.Map<MessageResponse<WorkshopResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<WorkshopResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }
    }
}
