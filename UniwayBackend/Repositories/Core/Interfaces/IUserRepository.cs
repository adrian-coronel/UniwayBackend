using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        Task<User?> FindByUsernameAndPassword(string Email, string Password);
        Task<User?> FindByIdAndRoleId(Guid Id, short RoleId);
        Task<User?> FindByRequestId(int RequestId);
        Task<User?> FindByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<List<User>> FindByAvailabilityAndLocation(Point point, short availabilityId = 0, int distance = 0);
        Task<List<User>> FindByListTechnicalProfessionAvailabilityId(List<int> techProfAvailabilities);
    }
}
