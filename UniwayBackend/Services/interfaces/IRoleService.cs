using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IRoleService
    {
        Task<MessageResponse<Role>> GetAll();
        Task<MessageResponse<Role>> GetById(short Id);
        Task<MessageResponse<Role>> Save(Role Role);
        Task<MessageResponse<Role>> Update(Role Role);
        Task<MessageResponse<Role>> Delete(short Id);
    }
}
