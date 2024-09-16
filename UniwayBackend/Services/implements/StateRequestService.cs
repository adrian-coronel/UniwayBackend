using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class StateRequestService : IStateRequestService
    {
        private readonly ILogger<StateRequest> _logger;
        private readonly IBaseRepository<StateRequest, short> _repository;
        private readonly UtilitariesResponse<StateRequest> _utilitaries;

        public StateRequestService(ILogger<StateRequest> logger, IBaseRepository<StateRequest, short> repository, UtilitariesResponse<StateRequest> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<StateRequest>> GetAll()
        {
            MessageResponse<StateRequest> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindAll();

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
