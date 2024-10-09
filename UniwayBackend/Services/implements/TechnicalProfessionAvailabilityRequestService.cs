using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class TechnicalProfessionAvailabilityRequestService : ITechnicalProfessionAvailabilityRequestService
    {
        private readonly ILogger<TechnicalProfessionAvailabilityRequestService> _logger;
        private readonly UtilitariesResponse<TechnicalProfessionAvailabilityRequestResponse> _utilitaries;
        private readonly ITechnicalProfessionAvailabilityRequestRepository _repository;

        public TechnicalProfessionAvailabilityRequestService(ILogger<TechnicalProfessionAvailabilityRequestService> logger,
                                                             UtilitariesResponse<TechnicalProfessionAvailabilityRequestResponse> utilitaries,
                                                             ITechnicalProfessionAvailabilityRequestRepository repository)
        {
            _logger = logger;
            _utilitaries = utilitaries;
            _repository = repository;
        }

        public async Task<MessageResponse<TechnicalProfessionAvailabilityRequestResponse>> GetAllPendingByUserId(Guid UserId)
        {
            MessageResponse<TechnicalProfessionAvailabilityRequestResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindAllPendingByUserId(UserId);

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
