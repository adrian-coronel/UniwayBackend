using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class AuthService : IAuthService
    {

        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _repository;
        private readonly UtilitariesResponse<User> _utilitaries;
        private readonly UserFactory _factory;

        public AuthService(IJwtService jwtService, ILogger<AuthService> logger, IUserRepository repository, UtilitariesResponse<User> utilitaries, UserFactory factory)
        {
            _jwtService = jwtService;
            _logger = logger;
            _repository = repository;
            _utilitaries = utilitaries;
            _factory = factory;
        }

        public async Task<AuthenticateResponse<User>> Authenticate([FromBody] AuthenticateRequest AuthRequest)
        {
            AuthenticateResponse<User> response;
            try
            {
                User? user = await _repository
                    .FindByUsernameAndPassword(AuthRequest.Email, AuthRequest.Password);

                if (user is null) return _utilitaries.setResponseBaseForNotFoundAuthenticate();

                string token = await _jwtService.GenerateJwtToken(user);

                response = _utilitaries.setResponseBaseForToken(token, user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                response = _utilitaries.setResponseBaseForAuthException(ex);
            }
            return response;
        }

        public async Task<AuthenticateResponse<User>> Register(RegisterRequest request)
        {
            AuthenticateResponse<User> response;
            try
            {
                User userSaved = await _factory.GetUser(request.RoleId).Create(request);

                string token = await _jwtService.GenerateJwtToken(userSaved);

                response = _utilitaries.setResponseBaseForToken(token, userSaved);
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                response = _utilitaries.setResponseBaseForAuthException(ex);
            }
            return response;
        }

        
        

    }
}
