using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;

namespace UniwayBackend.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<MessageResponse<User>> GetById(Guid Id);
        Task<MessageResponse<User>> Update(ProfileRequest request);
    }
}
