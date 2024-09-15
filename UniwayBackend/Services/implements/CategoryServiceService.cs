using Microsoft.Extensions.Logging;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class CategoryServiceService : ICategoryServiceService
    {

        public readonly ICategoryServiceRepository _repository;
        public readonly UtilitariesResponse<CategoryService> _utilitaries;
        public readonly ILogger<CategoryServiceService> _logger;

        public CategoryServiceService(ICategoryServiceRepository repository, UtilitariesResponse<CategoryService> utilitaries, ILogger<CategoryServiceService> logger)
        {
            _repository = repository;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        public async Task<MessageResponse<CategoryService>> GetAllByIdAndTechnicalProfessionAvailabilityId(short CategoryServiceId, int TechnicalProfessionAvailabilityId)
        {
            MessageResponse<CategoryService> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindByIdAndTechnicalProfessionAvailabilityId(CategoryServiceId, TechnicalProfessionAvailabilityId);

                if (result is null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<CategoryService>> GetAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId)
        {
            MessageResponse<CategoryService> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindAllByTechnicalProfessionAvailabilityId(TechnicalProfessionAvailabilityId);

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
