using Azure.Core;
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

        public async Task<MessageResponse<TechnicalResponse>> GetAllByClientIdAndRequestId(int ClientId, int RequestId)
        {
            MessageResponse<TechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var technicalResponses = await _repository.FindAllByClientIdAndRequest(ClientId, RequestId);

                response = _utilitaries.setResponseBaseForList(technicalResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<TechnicalResponse>> GetAllByRequestId(int RequestId)
        {
            MessageResponse<TechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var technicalResponses = await _repository.FindAllByRequestId(RequestId);

                response = _utilitaries.setResponseBaseForList(technicalResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<TechnicalResponse>> Save(TechnicalResponse technicalResponse)
        {
            MessageResponse<TechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (technicalResponse.Id > 0)
                    return _utilitaries.setResponseBaseForBadRequest("Se esta pasando el Id al intentar crear");

                if (technicalResponse.TechnicalProfessionAvailabilityId == 0) technicalResponse.TechnicalProfessionAvailabilityId = null;
                if (technicalResponse.WorkshopTechnicalProfessionId == 0) technicalResponse.WorkshopTechnicalProfessionId = null;


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
