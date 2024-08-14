using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using UniwayBackend.Config;
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
        private readonly IBaseRepository<UserTechnical, int> _userTechnicalRepository;
        public TechnicalCreator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TechnicalCreator>();
            _userRepository = new UserRepository();
            _technicalRepository = new TechnicalRepository();
            _userTechnicalRepository = new UserTechnicalRepository();
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
                    Lat = request.Lat,
                    Lng = request.Lng,
                    //WorkingStatus = Constants.State.INACTIVE_BOOL,
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

        public int GetRoleId()
        {
            return this.RoleId;
        }
    }
}
