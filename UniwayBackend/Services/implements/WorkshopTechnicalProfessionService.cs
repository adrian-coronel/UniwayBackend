using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class WorkshopTechnicalProfessionService : IWorkshopTechnicalProfessionService
    {
        private readonly IWorkshopTechnicalProfessionRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly UtilitariesResponse<WorkshopTechnicalProfession> _utilitaries;
        private readonly ILogger<WorkshopTechnicalProfessionService> _logger;

        public WorkshopTechnicalProfessionService(IWorkshopTechnicalProfessionRepository repository, IUserRepository userRepository, UtilitariesResponse<WorkshopTechnicalProfession> utilitaries, ILogger<WorkshopTechnicalProfessionService> logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        public async Task<MessageResponse<WorkshopTechnicalProfession>> Save(WorkshopTechnicalProfession WorkshopTechnicalProfession)
        {
            MessageResponse<WorkshopTechnicalProfession> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                WorkshopTechnicalProfession.Enabled = true;

                bool IsSaved = await _repository.Insert(WorkshopTechnicalProfession);

                response = IsSaved
                    ? _utilitaries.setResponseBaseForObject(WorkshopTechnicalProfession)
                    : _utilitaries.setResponseBaseForInternalServerError();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
