using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IWorkshopTechnicalProfessionService
    {
        Task<MessageResponse<WorkshopTechnicalProfession>> Save(WorkshopTechnicalProfession WorkshopTechnicalProfession);
    }
}
