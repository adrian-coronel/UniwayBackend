﻿using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService service, IMapper mapper, ILogger<RoleController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<MessageResponse<RoleResponse>>> GetAll()
        {
            MessageResponse<RoleResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                
                var result = await _service.GetAll();

                response = _mapper.Map<MessageResponse<Role>, MessageResponse<RoleResponse>>(result);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RoleResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<MessageResponse<RoleResponse>>> GetById(short Id)
        {
            MessageResponse<RoleResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetById(Id);

                response = _mapper.Map<MessageResponse<Role>, MessageResponse<RoleResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RoleResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse<RoleResponse>>> Save([FromBody] Role Role)
        {
            MessageResponse<RoleResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                
                var result = await _service.Save(Role);

                response = _mapper.Map<MessageResponse<Role>, MessageResponse<RoleResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RoleResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPut]
        public async Task<ActionResult<MessageResponse<RoleResponse>>> Update([FromBody] Role role)
        {
            MessageResponse<RoleResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                var result = await _service.Update(role);

                response = _mapper.Map<MessageResponse<Role>, MessageResponse<RoleResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RoleResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<MessageResponse<RoleResponse>>> Delete(short Id)
        {
            MessageResponse<RoleResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.Delete(Id);

                response = _mapper.Map<MessageResponse<Role>, MessageResponse<RoleResponse>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<RoleResponse>()
                   .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

    }
}
