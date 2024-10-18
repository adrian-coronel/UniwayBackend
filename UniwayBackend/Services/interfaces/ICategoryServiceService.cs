using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ICategoryServiceService
    {
        Task<MessageResponse<CategoryService>> GetAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<MessageResponse<CategoryService>> GetAllByIdAndAvailabilityId(short? CategoryServiceId = null, int? TechnicalProfessionAvailabilityId = null, short? AvailabilityId = null);
    }
}
