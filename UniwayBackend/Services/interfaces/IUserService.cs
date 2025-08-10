using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Request.Users;

namespace UniwayBackend.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<MessageResponse<User>> GetById(Guid Id);
        Task<MessageResponse<User>> Update(ProfileRequest request);
        Task<MessageResponse<User>> Delete(Guid userId, int roleId);
        Task<MessageResponse<User>> SaveAll(List<UserInsertRequest> users);
    }
}
