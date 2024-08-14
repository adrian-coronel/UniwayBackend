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

        public int GetRoleId()
        {
            return this.RoleId;
        }
    }
}
