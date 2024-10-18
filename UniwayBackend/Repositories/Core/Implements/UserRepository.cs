using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public async Task<List<User>> FindByAvailabilityAndLocation(Point point, short availabilityId = 0, int distance = 0)
        {
            using (var context = new DBContext())
            {
                return await context.Set<TechnicalProfessionAvailability>()
                    .Where(x =>
                        (
                            // Condiciones para AT_HOME o BOTH
                            (availabilityId == Constants.Availabilities.AT_HOME_ID || availabilityId == Constants.Availabilities.BOTH_ID)
                            &&
                            x.TechnicalProfession.UserTechnical.Technical.Location != null
                            && x.TechnicalProfession.UserTechnical.Technical.Location.Distance(point) <= distance
                            && x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL
                            && !(context.Requests
                                   .Any(r => r.TechnicalProfessionAvailability.Id == x.Id
                                             && r.StateRequestId == Constants.StateRequests.IN_PROCESS)
                               )
                        )
                        ||
                        (
                            // Condiciones para IN_WORKSHOP o BOTH
                            (availabilityId == Constants.Availabilities.IN_WORKSHOP_ID || availabilityId == Constants.Availabilities.BOTH_ID)
                            &&
                            x.Workshops.Any(w => w.Location != null && w.Location.Distance(point) <= distance)
                            && x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL
                        )
                    )
                    .Select(x => x.TechnicalProfession.UserTechnical.User)
                    .ToListAsync();


            }
        }

        public async Task<List<User>> FindByListTechnicalProfessionAvailabilityId(List<int> techProfAvailabilities)
        {
            using (DBContext context = new DBContext())
            {
                return await context.TechnicalProfessionAvailabilities
                    .Where(tpa => techProfAvailabilities.Contains(tpa.Id)) // Filtra por los IDs proporcionados
                    .Select(tpa => tpa.TechnicalProfession.UserTechnical.User) // Selecciona el usuario relacionado
                    .Distinct() // Elimina posibles duplicados
                    .ToListAsync(); // Ejecuta la consulta y retorna la lista
            }
        }


        public async Task<User?> FindByIdAndRoleId(Guid Id, short RoleId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<User>()
                    .SingleOrDefaultAsync(u => u.Id == Id && u.RoleId == RoleId);
            }
        }

        public async Task<User?> FindByTechnicalProfessionAvailabilityId(int technicalProfessionAvailabilityId)
        {
            using (var context = new DBContext())
            {
                var query = from user in context.Users
                            join userTechnical in context.UserTechnicals 
                                on user.Id equals userTechnical.UserId
                            join technicalProfession in context.TechnicalProfessions 
                                on userTechnical.Id equals technicalProfession.UserTechnicalId
                            join technicalProfessionAvailability in context.TechnicalProfessionAvailabilities
                                on technicalProfession.Id equals technicalProfessionAvailability.TechnicalProfessionId
                            where technicalProfessionAvailability.Id == technicalProfessionAvailabilityId
                            select user;

                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<User?> FindByUsernameAndPassword(string Email, string Password)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<User>()
                    .SingleOrDefaultAsync(u => u.Email == Email && u.Password == Password);
            }
        }

        public async Task<User?> FindByRequestId(int RequestId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<Request>()
                    .Where(x => x.Id == RequestId)
                    .Select(x => x.Client.User)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<User?> FindByClientId(int ClientId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.clients
                    .Where(x => x.Id == ClientId)
                    .Select(x => x.User)
                    .FirstAsync();
            }
        }

        public async Task<DataUserResponse> FindTechnicalOrWorkshop(int TechProfAvaiId)
        {
            using (DBContext context = new DBContext())
            {
                DataUserResponse response = new DataUserResponse();
                var techProfAvai = await context.TechnicalProfessionAvailabilities.FindAsync(TechProfAvaiId);

                if (techProfAvai.AvailabilityId == Constants.Availabilities.BOTH_ID)
                {
                    var technical = await context.TechnicalProfessionAvailabilities
                        .Where(x => x.Id == techProfAvai.Id)
                        .Select(x => x.TechnicalProfession.UserTechnical.Technical)
                        .FirstAsync();

                    response.EntityId = technical.Id.ToString();
                    response.FullName = $"{technical.Name} {technical.FatherLastname} {technical.MotherLastname}";
                    response.TypeEntity = Constants.EntityTypes.MECHANICAL;
                }
                if (techProfAvai.AvailabilityId == Constants.Availabilities.IN_WORKSHOP_ID)
                {
                    var workshop = await context.TechnicalProfessionAvailabilities
                        .Where(x => x.Id == techProfAvai.Id)
                        .Select(x => x.Workshops.First())
                        .FirstAsync();

                    response.EntityId = workshop.Id.ToString();
                    response.FullName = $"{workshop.Name}";
                    response.TypeEntity = Constants.EntityTypes.WORKSHOP;
                }
                return response;
            }
        }
    }
}
