using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class TechnicalProfessionAvailabilityService : ITechnicalProfessionAvailabilityService
    {

        private readonly ITechnicalProfessionAvailabilityRepository _repository;
        private readonly ILogger<TechnicalProfessionAvailability> _logger;
        private readonly UtilitariesResponse<TechnicalProfessionAvailability> _utilitaries;

        public TechnicalProfessionAvailabilityService(ITechnicalProfessionAvailabilityRepository repository, ILogger<TechnicalProfessionAvailability> logger, UtilitariesResponse<TechnicalProfessionAvailability> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<TechnicalProfessionAvailability>> GetByTechnicalAndAvailability(int TechnicalId, short AvailabilityId)
        {
            MessageResponse<TechnicalProfessionAvailability> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                TechnicalProfessionAvailability? technicalAvailability = await _repository.FindByTechnicalAndAvailability(TechnicalId, AvailabilityId);

                if (technicalAvailability == null) return _utilitaries.setResponseBaseForNotFount();

                response = _utilitaries.setResponseBaseForObject(technicalAvailability);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        //public Task<MessageResponse<TechnicalProfessionAvailability>> GetAllTechnicalLocations(int RangeDistance)
        //{
        //    MessageResponse<TechnicalProfessionAvailability> response;
        //    try
        //    {
        //        _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

        //        var technicals = _repository.FindAllByWorkshopStatusAndAvailability(true, 1);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
