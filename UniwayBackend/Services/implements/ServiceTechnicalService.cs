using AutoMapper;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ServiceTechnicalService : IServiceTechnicalService
    {

        private readonly ILogger<ServiceTechnicalService> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceTechnicalRepository _repository;
        private readonly UtilitariesResponse<ServiceTechnical> _utilitaries;

        public ServiceTechnicalService(ILogger<ServiceTechnicalService> logger, IMapper mapper, IServiceTechnicalRepository repository, UtilitariesResponse<ServiceTechnical> utilitaries)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<ServiceTechnical>> GetByTechnicaIdAndAvailabilityId(int technicalId, short availabilityId)
        {
            MessageResponse<ServiceTechnical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository
                                .FindByTechnicalIdAndAvailabilityId(technicalId, availabilityId);

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
