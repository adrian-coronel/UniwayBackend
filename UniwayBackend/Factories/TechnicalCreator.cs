using Azure.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    public class TechnicalCreator : IUser
    {
        private readonly int RoleId = Constants.Roles.TECHNICAL_ID;

        private readonly ILogger<TechnicalCreator> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IUserTechnicalRepository _userTechnicalRepository;
        public TechnicalCreator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TechnicalCreator>();
            _userRepository = new UserRepository();
            _technicalRepository = new TechnicalRepository();
            _userTechnicalRepository = new UserTechnicalRepository();
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

                // Creación del User
                User user = new User
                {
                    Email = request.Email,
                    Password = request.Password,
                    RoleId = request.RoleId,
                    Enabled = Constants.State.ACTIVE_BOOL,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };

                user = await _userRepository.InsertAndReturn(user);

                // Creación del Technical
                Technical technical = new Technical
                {
                    Name = request.Name,
                    FatherLastname = request.FatherLastname,
                    MotherLastname = request.MotherLastname,
                    Dni = request.Dni,
                    PhoneNumber = request.PhoneNumber,
                    BirthDate = request.BirthDate,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                technical = await _technicalRepository.InsertAndReturn(technical);

                UserTechnical userTechnical = new UserTechnical
                {
                    UserId = user.Id,
                    TechnicalId = technical.Id,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                await _userTechnicalRepository.Insert(userTechnical);

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

                UserTechnical? userTechnical = await _userTechnicalRepository.FindByUserIdAndRoleId(request.UserId, request.RoleId);

                if (userTechnical is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {request.UserId} e ID de rol {request.RoleId}");

                userTechnical.User.Password = !string.IsNullOrEmpty(request.Password) ? request.Password : userTechnical.User.Password;
                userTechnical.Technical.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : userTechnical.Technical.Name;
                userTechnical.Technical.FatherLastname = !string.IsNullOrEmpty(request.FatherLastname) ? request.FatherLastname : userTechnical.Technical.FatherLastname;
                userTechnical.Technical.MotherLastname = !string.IsNullOrEmpty(request.MotherLastname) ? request.MotherLastname : userTechnical.Technical.MotherLastname;
                userTechnical.Technical.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : userTechnical.Technical.PhoneNumber;
                userTechnical.Technical.BirthDate = request.BirthDate.HasValue ? request.BirthDate.Value : userTechnical.Technical.BirthDate;

                userTechnical = await _userTechnicalRepository.UpdateAndReturn(userTechnical);

                return userTechnical.User;
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
                UserTechnical? userTechnical = await _userTechnicalRepository.FindByUserIdAndRoleId(UserId, GetRoleId());

                if (userTechnical is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {UserId} e ID de rol {GetRoleId()}");

                userTechnical.User.Enabled = false;
                userTechnical.Enabled = false;

                userTechnical = await _userTechnicalRepository.UpdateAndReturn(userTechnical);

                return userTechnical.User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }
}
