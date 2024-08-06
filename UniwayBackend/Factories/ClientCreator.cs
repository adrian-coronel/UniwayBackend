using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Auth;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Factories
{
    public class ClientCreator : IUser
    {
        private readonly int RoleId = Constants.Roles.CLIENT_ID;

       

        public async Task<User> Create(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public int GetRoleId()
        {
            return this.RoleId;
        }
    }
}
