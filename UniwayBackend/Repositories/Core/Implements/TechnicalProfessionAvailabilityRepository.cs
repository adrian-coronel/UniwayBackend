using Microsoft.EntityFrameworkCore;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalProfessionAvailabilityRepository : BaseRepository<TechnicalProfessionAvailability, int>, ITechnicalProfessionAvailabilityRepository
    {
        //public async Task<List<TechnicalProfessionAvailability>> FindAllByWorkshopStatusAndAvailability(bool WorkingStatus, short AvailabilityId)
        //{
        //    using (var context = new DBContext())
        //    {
        //        return await context.Set<TechnicalProfessionAvailability>()
        //            .Where(x => x.AvailabilityId == AvailabilityId 
        //                     && x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == WorkingStatus
        //                     && x.TechnicalProfession.UserTechnical.User.RoleId == Constants.Roles.TECHNICAL_ID)
        //            .ToListAsync();
        //    }
        //}
    }
}
