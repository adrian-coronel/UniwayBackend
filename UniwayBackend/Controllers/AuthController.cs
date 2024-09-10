using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IAuthService service, ILogger<AuthController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthenticateResponse<User>>> Register([FromBody] RegisterRequest request)
        {
            AuthenticateResponse<User> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                response = await _service.Register(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new AuthenticateResponseBuilder<User>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticateResponse<User>>> Authenticate([FromBody] AuthenticateRequest request)
        {
            AuthenticateResponse<User> response; 
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                response = await _service.Authenticate(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new AuthenticateResponseBuilder<User>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }
    }
}
