using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class RequestService : IRequestService
    {
        private readonly ILogger<RequestService> _logger;
        private readonly IRequestRepository _repository;
        private readonly UtilitariesResponse<Request> _utilitaries;

        public RequestService(ILogger<RequestService> logger, IRequestRepository repository, UtilitariesResponse<Request> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Request>> GetRequestPendingInTrayByUserId(Guid userId)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var requests = await _repository.FindAllPendingByUserId(userId);

                response = _utilitaries.setResponseBaseForList(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Request>> Save(Request request)
        {
            MessageResponse<Request> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (request.Id != 0) return _utilitaries.setResponseBaseForInternalServerError();

                Request requestSaved = await _repository.InsertAndReturn(request);

                response = _utilitaries.setResponseBaseForObject(requestSaved);
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
