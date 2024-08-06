using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;
using UniwayBackend.Models.Payloads.Auth;

namespace UniwayBackend.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<MessageResponse<User>> GetById(Guid Id);
    }
}
