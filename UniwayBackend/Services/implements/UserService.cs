using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly UtilitariesResponse<User> _utilitaries;

        public UserService(IUserRepository repository, ILogger<UserService> logger, UtilitariesResponse<User> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
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

                if (user == null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(user);
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
