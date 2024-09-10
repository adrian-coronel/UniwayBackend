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
