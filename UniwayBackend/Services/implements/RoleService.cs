using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class RoleService : IRoleService
    {
        
        private readonly IRoleRepository _repository;
        private readonly UtilitariesResponse<Role> _utilitaries;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository repository, UtilitariesResponse<Role> utilitaries, ILogger<RoleService> logger)
        {
            _repository = repository;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        public async Task<MessageResponse<Role>> GetAll()
        {
            MessageResponse<Role> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                List<Role> rols = await _repository.FindAll();

                response = _utilitaries.setResponseBaseForList(rols);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;            
        }

        public async Task<MessageResponse<Role>> GetById(short Id)
        {
            MessageResponse<Role> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                Role? role = await _repository.FindById(Id);

                if (role == null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(role);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }


        public async Task<MessageResponse<Role>> Save(Role Role)
        {
            MessageResponse<Role> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                
                bool IsSaved = await _repository.Insert(Role);

                if (!IsSaved)
                    return _utilitaries.setResponseBaseForInternalServerError();

                response = _utilitaries.setResponseBaseForObject(Role);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Role>> Update(Role Role)
        {
            MessageResponse<Role> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                Role? role = await _repository.FindById(Role.Id);

                if (role == null) return _utilitaries.setResponseBaseNotFoundForUpdate();

                role!.Name = Role.Name;

                bool IsUpdated = await _repository.Update(role);

                if (!IsUpdated) return _utilitaries.setResponseBaseForInternalServerError();

                response = _utilitaries.setResponseBaseForObject(role);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Role>> Delete(short Id)
        {
            MessageResponse<Role> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                Role? role = await _repository.FindById(Id);

                if (role == null) return _utilitaries.setResponseBaseNotFoundForDelete();

                bool IsDeleted = await _repository.Delete(Id);

                if (!IsDeleted) return _utilitaries.setResponseBaseForInternalServerError();

                response = _utilitaries.setResponseBaseForOk();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

    }
}
