using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Exceptions;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Factories
{
    public class EmployeeCreator : IUser
    {
        private readonly int RoleId = Constants.Roles.EMPLOYEE_ID;

        private readonly ILogger<EmployeeCreator> _logger;
        private readonly IUserTechnicalRepository _userTechnicalRepository;
        private readonly IUserRepository _userRepository;

        public EmployeeCreator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmployeeCreator>();
            _userTechnicalRepository = new UserTechnicalRepository();
            _userRepository = new UserRepository();
        }

        public async Task<User> Create(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Validaciones

                // Verificamos que no exista un usuario para el tecnico
                if ( await _userTechnicalRepository.ExistsUserTypeWithTechnicalByDni(request.RoleId, request.Dni) )
                    throw new UserNoCreateException($"Cuenta de tipo empleado ya existente");
                // Verificamos que previamente exista un registro de Tecnico para crear el Empleado
                else if ( !await _userTechnicalRepository.ExistsTechnicalByDni(request.Dni))
                    throw new UserNoCreateException($"Necesita crear una cuenta como Técnico previamente");

                // Buscamos el tecnico
                Technical? technical = await _userTechnicalRepository.FindTechnicalByDni(request.Dni);

                // Creamos el usuario
                User user = await _userRepository.InsertAndReturn(new User
                    {
                        Email = request.Email,
                        Password = request.Password,
                        RoleId = request.RoleId,
                        Enabled = Constants.State.ACTIVE_BOOL,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                }
                );

                await _userTechnicalRepository.Insert( new UserTechnical
                    {
                        UserId = user.Id,
                        TechnicalId = technical.Id,
                        Enabled = Constants.State.ACTIVE_BOOL
                    }    
                );

                return user;
            } 
            catch(Exception ex)
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
                userTechnical.Technical.BirthDate = request.BirthDate.HasValue ? request.BirthDate.Value : userTechnical.Technical.BirthDate;
                userTechnical.Technical.Lat = request.Lat;
                userTechnical.Technical.Lng = request.Lng;

                userTechnical = await _userTechnicalRepository.UpdateAndReturn(userTechnical);

                return userTechnical.User;
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
