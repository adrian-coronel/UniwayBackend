using Azure.Core;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Request.Users;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly UtilitariesResponse<User> _utilitaries;
        private readonly UserFactory _userFactory;

        public UserService(IUserRepository repository, ILogger<UserService> logger, UtilitariesResponse<User> utilitaries, UserFactory userFactory)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
            _userFactory = userFactory;
        }

        

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<MessageResponse<User>> GetById(Guid Id)
        {
            MessageResponse<User> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                User? user = await _repository.FindById(Id);

                if (user is null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<User>> Update(ProfileRequest request)
        {
            MessageResponse<User> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                User? user = await _userFactory.GetUser(request.RoleId).Edit(request);

                if (user is null) return _utilitaries.setResponseBaseForInternalServerError();

                return _utilitaries.setResponseBaseForObject(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<User>> Delete(Guid userId, int roleId)
        {
            MessageResponse<User> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                User? user = await _userFactory.GetUser(roleId).Delete(userId);

                if (user is null) return _utilitaries.setResponseBaseForInternalServerError();

                return _utilitaries.setResponseBaseForObject(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<User>> SaveAll(List<UserInsertRequest> users)
        {
            MessageResponse<User> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                List<User> entities = users.Select(x => new User
                {
                    Id = Guid.NewGuid(),
                    RoleId = x.RoleId,
                    Email = x.Email,
                    Password = x.Password,
                    Enabled = Constants.State.ACTIVE_BOOL,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                }).ToList();

                var result = await _repository.InsertAll(entities);

                response = _utilitaries.setResponseBaseForList(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
