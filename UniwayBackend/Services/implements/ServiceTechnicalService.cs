using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ServiceTechnicalService : IServiceTechnicalService
    {

        public readonly ILogger<ServiceTechnicalService> _logger;
        

        public async Task<MessageResponse<ServiceTechnical>> GetByTechnicalAvailabilityId(int TechnicalAvailabilityId)
        {
            throw new NotImplementedException();
        }
    }
}
