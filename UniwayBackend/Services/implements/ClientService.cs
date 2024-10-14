using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly ILogger<ClientService> _logger;
        private readonly UtilitariesResponse<Client> _utilitaries;

        public ClientService(IClientRepository repository, ILogger<ClientService> logger, UtilitariesResponse<Client> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Client>> GetById(int Id)
        {
            MessageResponse<Client> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var client = await _repository.FindById(Id);

                if (client == null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Client>> GetInformationByUser(Guid UserId)
        {
            MessageResponse<Client> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var client = await _repository.FindByUserId(UserId);

                if (client == null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(client);
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
