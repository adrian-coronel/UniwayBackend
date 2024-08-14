using Microsoft.Extensions.Logging;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Factories
{
    public class ClientCreator : IUser
    {
        private readonly int RoleId = Constants.Roles.CLIENT_ID;
        
        private readonly ILogger<ClientCreator> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;

        public ClientCreator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClientCreator>();
            _userRepository = new UserRepository();
            _clientRepository = new ClientRepository();
        }

        public async Task<User> Create(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                User user = new User
                {
                    RoleId = request.RoleId,
                    Email = request.Email,
                    Password = request.Password,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                user = await _userRepository.InsertAndReturn(user);

                Client client = new Client
                {
                    UserId = user.Id,
                    Name = request.Name,
                    FatherLastname = request.FatherLastname,
                    MotherLastname = request.MotherLastname,
                    Dni = request.Dni,
                    BirthDate = request.BirthDate,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                await _clientRepository.Insert(client);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
        }

        public int GetRoleId()
        {
            return this.RoleId;
        }
    }
}
