using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<MessageResponse<User>> GetById(Guid Id);
    }
}
