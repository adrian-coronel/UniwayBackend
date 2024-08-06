using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IUserTechnicalRepository : IBaseRepository<UserTechnical, int>
    {

        Task<bool> ExistsTechnicalByDni(string Dni);

        Task<bool> ExistsUserTypeWithTechnicalByDni(short RoleId, string Dni);

        Task<Technical?> FindTechnicalByDni(string Dni);

    }
}
