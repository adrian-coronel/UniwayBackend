using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class TechnicalService : ITechnicalService
    {

        public readonly ITechnicalRepository _repository;
        public readonly ILogger<TechnicalService> _logger;
        public readonly UtilitariesResponse<Technical> _utilitaries;

        public TechnicalService(ITechnicalRepository repository, ILogger<TechnicalService> logger, UtilitariesResponse<Technical> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Technical>> GetInformation(int TechnicalId)
        {
            MessageResponse<Technical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindTechnicalWithInformation(TechnicalId);

                response = _utilitaries.setResponseBaseForObject(result);
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
