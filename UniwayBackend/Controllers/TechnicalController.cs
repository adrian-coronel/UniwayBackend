﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Technical;
using UniwayBackend.Models.Payloads.Core.Response.Technical;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalController : ControllerBase
    {
        public readonly ILogger<TechnicalController> _logger;
        public readonly ITechnicalService _service;
        public readonly IMapper _mapper;

        public TechnicalController(ILogger<TechnicalController> logger, ITechnicalService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        
        [HttpGet("GetInformation/{TechnicalId}")]
        public async Task<ActionResult<MessageResponse<TechnicalResponseV2>>> GetInformation(int TechnicalId)
        {
            MessageResponse<TechnicalResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetInformation(TechnicalId);

                response = _mapper.Map<MessageResponse<Technical>, MessageResponse<TechnicalResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<TechnicalResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetInformationByUser/{UserId}")]
        public async Task<ActionResult<MessageResponse<TechnicalResponseV2>>> GetInformationByUser(Guid UserId)
        {
            MessageResponse<TechnicalResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetInformationByUser(UserId);

                response = _mapper.Map<MessageResponse<Technical>, MessageResponse<TechnicalResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<TechnicalResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPut("UpdateWorkingStatus")]
        public async Task<ActionResult<MessageResponse<TechnicalResponseV1>>> UpdateWorkingStatus([FromBody] TechnicalRequestV1 request)
        {
            MessageResponse<TechnicalResponseV1> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.UpdateWorkinStatus(request);
                
                response = _mapper.Map<MessageResponse<Technical>, MessageResponse<TechnicalResponseV1>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<TechnicalResponseV1>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }
    }
}
