using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IServiceTechnicalService
    {
        Task<MessageResponse<ServiceTechnical>> GetById(int serviceTechnicalId);
        Task<MessageResponse<ServiceTechnical>> GetByTechnicaIdAndAvailabilityId(int technicalId, short availabilityId);
        Task<MessageResponse<ServiceTechnical>> Save(ServiceTechnical serviceTechnical, List<IFormFile> Files);
        Task<MessageResponse<ServiceTechnical>> Update(ServiceTechnical serviceTechnical);

    }
}
