using NetTopologySuite.Geometries;
using NetTopologySuite;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Services.implements
{
    public class LocationService : ILocationService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly ILogger<LocationService> _logger;
        private readonly UtilitariesResponse<LocationResponse> _utilitaries;

        public LocationService(ITechnicalRepository technicalRepository, IWorkshopRepository workshopRepository, ILogger<LocationService> logger, UtilitariesResponse<LocationResponse> utilitaries)
        {
            _technicalRepository = technicalRepository;
            _workshopRepository = workshopRepository;
            _logger = logger;
            _utilitaries = utilitaries;
        }




        // Si la disponibilidad es 0, se trae ambas disponibilidades
        public async Task<MessageResponse<LocationResponse>> GetAllByAvailability(LocationRequest request)
        {
            MessageResponse<LocationResponse> response;
            List<LocationResponse> results = new List<LocationResponse>();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var referenceLocation = new Point(request.Longitud, request.Latitud) { SRID = 4326 };

                if (request.AvailabilityId == Constants.Availabilities.AT_HOME_ID || request.AvailabilityId == Constants.Availabilities.BOTH_ID)
                {
                    var technicals = await _technicalRepository.FindDefaultLocation(referenceLocation, request.Distance);
                    results.AddRange(
                        technicals.Select(tech => new LocationResponse
                        {
                            Id = tech.Id.ToString(),
                            Name = $"{tech.Name} {tech.FatherLastname}",
                            Location = tech.Location,
                            WorkingStatus = tech.WorkingStatus,
                            IsWorkshop = false,
                        }).ToList()
                    );
                }
                if (request.AvailabilityId == Constants.Availabilities.IN_WORKSHOP_ID || request.AvailabilityId == Constants.Availabilities.BOTH_ID)
                {
                    var workshops = await _workshopRepository.FindDefaultLocation(referenceLocation, request.Distance);
                    results.AddRange(
                        workshops.Select(wor => new LocationResponse
                        {
                            Id = wor.Id.ToString(),
                            Name = wor.Name,
                            Location = wor.Location,
                            WorkingStatus = wor.WorkingStatus,
                            IsWorkshop = true,
                        }).ToList()    
                    );
                }

                response = _utilitaries.setResponseBaseForList(results);        
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
