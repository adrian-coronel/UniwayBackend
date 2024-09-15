using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ICategoryServiceService
    {
        Task<MessageResponse<CategoryService>> GetAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<MessageResponse<CategoryService>> GetAllByIdAndTechnicalProfessionAvailabilityId(short CategoryServiceId, int TechnicalProfessionAvailabilityId);
    }
}
