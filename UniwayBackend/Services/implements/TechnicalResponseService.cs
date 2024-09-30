using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class TechnicalResponseService : ITechnicalResponseService
    {

        private readonly ILogger<TechnicalResponseService> _logger;
        private readonly UtilitariesResponse<TechnicalResponse> _utilitaries;
        private readonly ITechnicalResponseRepository _repository;

        public TechnicalResponseService(ILogger<TechnicalResponseService> logger, UtilitariesResponse<TechnicalResponse> utilitaries, ITechnicalResponseRepository repository)
        {
            _logger = logger;
            _utilitaries = utilitaries;
            _repository = repository;
        }

        public async Task<MessageResponse<TechnicalResponse>> Save(TechnicalResponse technicalResponse)
        {
            MessageResponse<TechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (technicalResponse.Id > 0)
                    return _utilitaries.setResponseBaseForBadRequest("Se esta pasando el Id al intentar crear");

                var result = await _repository.InsertAndReturn(technicalResponse);

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
