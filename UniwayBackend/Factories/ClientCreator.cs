using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Exceptions;
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

        public int GetRoleId()
        {
            return this.RoleId;
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
                    PhoneNumber = request.PhoneNumber,
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

        public async Task<User> Edit(ProfileRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                Client? client = await _clientRepository.FindByUserId(request.UserId);

                if (client is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {request.UserId} e ID de rol {request.RoleId}");

                client.User.Password = !string.IsNullOrEmpty(request.Password) ? request.Password : client.User.Password;
                client.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : client.Name;
                client.FatherLastname = !string.IsNullOrEmpty(request.FatherLastname) ? request.FatherLastname : client.FatherLastname;
                client.MotherLastname = !string.IsNullOrEmpty(request.MotherLastname) ? request.MotherLastname : client.MotherLastname;
                client.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : client.PhoneNumber;
                client.BirthDate = request.BirthDate.HasValue ? request.BirthDate.Value : client.BirthDate;

                client = await _clientRepository.UpdateAndReturn(client);

                return client.User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<User> Delete(Guid UserId)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                User? user = await _userRepository.FindById(UserId);

                if (user is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {UserId} e ID de rol {GetRoleId()}");

                user.Enabled = false;

                return await _userRepository.UpdateAndReturn(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
