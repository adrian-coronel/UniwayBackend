﻿using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IServiceTechnicalRepository : IBaseRepository<ServiceTechnical, int>
    {
        Task<ServiceTechnical?> GetByIdWithInformation(int ServiceTechnicalId);
        Task<List<ServiceTechnical>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<List<ServiceTechnical>> FindFiveByTechnicalId(int TechnicalId);
        Task<List<ServiceTechnical>> FindByTechnicalIdAndAvailabilityId(int TechnicalId, short AvailabilityId);
        Task<List<ServiceTechnical>> FindFiveByWorkshopId(int WorkshopId);
        Task<TechnicalProfessionAvailability> FindTechnicalProfessionAvailibiltyByServiceId(int serviceTechnicalId);
    }
}
