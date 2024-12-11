using Microsoft.EntityFrameworkCore;
using System.Net;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using static Amazon.S3.Util.S3EventNotification;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class UserTechnicalRepository : BaseRepository<UserTechnical, int>, IUserTechnicalRepository
    {
        public async Task<bool> ExistsTechnicalByDni(string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<Technical>()
                    .AnyAsync(t => t.Dni == Dni);
            }
        }

        public async Task<bool> ExistsUserTypeWithTechnicalByDni(short RoleId, string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<UserTechnical>()
                    .AnyAsync(ut => ut.User.RoleId == RoleId && 
                                    ut.Technical.Dni == Dni &&
                                    ut.Enabled);
            }
        }

        public async Task<UserTechnical?> FindByUserIdAndRoleId(Guid userId, int roleId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<UserTechnical>()
                    .Include(x => x.User)
                    .Include(x => x.Technical)
                    .SingleOrDefaultAsync(x => x.User.Id == userId && x.User.RoleId == roleId);
            }
        }

        public async Task<Technical?> FindTechnicalByDni(string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<Technical>()
                    .SingleOrDefaultAsync(t => t.Dni == Dni);
            }
        }

        public async Task<bool> Insert(UserTechnical userTechnical)
        {
            using (DBContext context = new DBContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Agregar el objeto principal
                        await context.AddAsync(userTechnical);
                        await context.SaveChangesAsync();

                        if (userTechnical.Id == 0 || userTechnical.Id == null) return false;

                        // Crear y agregar la profesión técnica
                        var technicalProfession = new TechnicalProfession
                        {
                            ExperienceId = Constants.Experiences.Principiante,
                            ProfessionId = Constants.Professions.Electricista,
                            UserTechnicalId = userTechnical.Id,
                        };

                        await context.AddAsync(technicalProfession);
                        await context.SaveChangesAsync();

                        if (technicalProfession.Id == 0 || technicalProfession.Id == null) return false;

                        // Crear y agregar la disponibilidad técnica
                        var technicalProfessionTechnical = new TechnicalProfessionAvailability
                        {
                            TechnicalProfessionId = technicalProfession.Id,
                            AvailabilityId = Constants.Availabilities.AT_HOME_ID
                        };

                        await context.AddAsync(technicalProfessionTechnical);
                        await context.SaveChangesAsync();

                        // Confirmar la transacción
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        // Revertir la transacción en caso de error
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }

    }
}
