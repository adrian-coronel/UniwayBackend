using NetTopologySuite.Geometries;
using System.Reflection;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Location;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using System.Threading.Tasks;

namespace UniwayBackend.Services.implements
{
    public class LocationService : ILocationService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IServiceTechnicalRepository _serviceTechnicalRepository;
        private readonly ILogger<LocationService> _logger;
        private readonly UtilitariesResponse<LocationResponse> _utilitaries;
        private readonly UtilitariesResponse<LocationResponseV2> _utilitariesV2;

        public LocationService(ITechnicalRepository technicalRepository,
                               IWorkshopRepository workshopRepository,
                               IServiceTechnicalRepository serviceTechnicalRepository,
                               ILogger<LocationService> logger,
                               UtilitariesResponse<LocationResponse> utilitaries,
                               UtilitariesResponse<LocationResponseV2> utilitariesV2)
        {
            _technicalRepository = technicalRepository;
            _workshopRepository = workshopRepository;
            _serviceTechnicalRepository = serviceTechnicalRepository;
            _logger = logger;
            _utilitaries = utilitaries;
            _utilitariesV2 = utilitariesV2;
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
                            AvailabilityId = Constants.Availabilities.AT_HOME_ID,
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
                            AvailabilityId = Constants.Availabilities.IN_WORKSHOP_ID,
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

        public async Task<MessageResponse<LocationResponseV2>> GetAllByAvailabilityWithServices(LocationRequest request)
        {
            MessageResponse<LocationResponseV2> response;
            List<LocationResponseV2> results = new List<LocationResponseV2>();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var referenceLocation = new Point(request.Longitud, request.Latitud) { SRID = 4326 };

                if (request.AvailabilityId == Constants.Availabilities.AT_HOME_ID || request.AvailabilityId == Constants.Availabilities.BOTH_ID)
                {
                    var technicals = await _technicalRepository.FindDefaultLocation(referenceLocation, request.Distance);

                    // Creamos una lista de tareas para manejar las operaciones asincrónicas
                    var tasks = technicals.Select(async tech => new LocationResponseV2
                    {
                        Id = tech.Id.ToString(),
                        Name = $"{tech.Name} {tech.FatherLastname}",
                        Location = tech.Location,
                        WorkingStatus = tech.WorkingStatus,
                        AvailabilityId = Constants.Availabilities.AT_HOME_ID,
                        IsWorkshop = false,
                        Services = (
                            await _serviceTechnicalRepository.FindFiveByTechnicalId(tech.Id)
                        ).Select(x => new ServiceTechnicalResponse
                        {
                            // Mapea aquí los campos de ServiceTechnical a ServiceTechnicalResponse
                        }).ToList()

                    }).ToList();

                    // Esperamos que todas las tareas se completen
                    var resultsFromTasks = await Task.WhenAll(tasks);

                    // Agregamos los resultados a la lista final
                    results.AddRange(resultsFromTasks);
                }

                if (request.AvailabilityId == Constants.Availabilities.IN_WORKSHOP_ID || request.AvailabilityId == Constants.Availabilities.BOTH_ID)
                {
                    var workshops = await _workshopRepository.FindDefaultLocation(referenceLocation, request.Distance);

                    var tasks = workshops.Select(async wor => new LocationResponseV2
                    {
                        Id = wor.Id.ToString(),
                        Name = wor.Name,
                        Location = wor.Location,
                        WorkingStatus = wor.WorkingStatus,
                        AvailabilityId = Constants.Availabilities.IN_WORKSHOP_ID,
                        IsWorkshop = true,

                        Services = (
                            await _serviceTechnicalRepository.FindFiveByWorkshopId(wor.Id)
                        ).Select(x => new ServiceTechnicalResponse
                        {
                            // Mapea aquí los campos de ServiceTechnical a ServiceTechnicalResponse
                        }).ToList()
                    }).ToList();

                    // Esperamos que todas las tareas se completen
                    var resultsFromTasks = await Task.WhenAll(tasks);

                    // Agregamos los resultados a la lista final
                    results.AddRange(resultsFromTasks);
                }

                response = _utilitariesV2.setResponseBaseForList(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitariesV2.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
