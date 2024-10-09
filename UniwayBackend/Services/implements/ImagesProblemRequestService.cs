using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ImagesProblemRequestService : IImagesProblemRequestService
    {
        private readonly IImagesProblemRequestRepository _repository;
        private readonly ILogger<ImagesProblemRequestService> _logger;
        private readonly UtilitariesResponse<ImagesProblemRequest> _utilitaries;

        public ImagesProblemRequestService(IImagesProblemRequestRepository repository,
                                           ILogger<ImagesProblemRequestService> logger,
                                           UtilitariesResponse<ImagesProblemRequest> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<ImagesProblemRequest>> Save(ImagesProblemRequest imagesProblemRequest)
        {
            MessageResponse<ImagesProblemRequest> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (imagesProblemRequest.Id != 0) return _utilitaries.setResponseBaseForInternalServerError();

                var result = await _repository.InsertAndReturn(imagesProblemRequest);

                response = _utilitaries.setResponseBaseForObject(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ImagesProblemRequest>> SaveAll(List<ImagesProblemRequest> imagesProblemRequests)
        {
            MessageResponse<ImagesProblemRequest> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (imagesProblemRequests.Any(x => x.Id != 0)) return _utilitaries.setResponseBaseForInternalServerError();

                var result = await _repository.InsertAll(imagesProblemRequests);

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
