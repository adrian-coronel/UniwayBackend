using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class CategoryRequestService : ICategoryRequestService
    {
        private readonly ILogger<CategoryRequestService> _logger;
        private readonly IBaseRepository<CategoryRequest, short> _repository;
        private readonly UtilitariesResponse<CategoryRequest> _utilitaries;

        public CategoryRequestService(ILogger<CategoryRequestService> logger, IBaseRepository<CategoryRequest, short> repository, UtilitariesResponse<CategoryRequest> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<CategoryRequest>> GetAll()
        {
            MessageResponse<CategoryRequest> response;
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
