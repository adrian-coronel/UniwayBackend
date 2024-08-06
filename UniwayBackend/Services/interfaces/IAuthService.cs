using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Auth;

namespace UniwayBackend.Services.interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateResponse<User>> Authenticate(AuthenticateRequest AuthRequest);
        Task<AuthenticateResponse<User>> Register(RegisterRequest request);
        //Task<IEnumerable<User>> GetAll();
        //Task<User?> GetById(int id);
        //Task<User?> AddAndUpdateUser(User userObj);
    }
}
