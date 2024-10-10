using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalProfessionAvailabilityRequestRepository : BaseRepository<TechnicalProfessionAvailabilityRequest, int>, ITechnicalProfessionAvailabilityRequestRepository
    {
        //public async Task<List<TechnicalProfessionAvailabilityRequestResponse>> FindAllPendingByUserId(Guid UserId)
        //{
        //    using (DBContext context = new DBContext())
        //    {
        //        // Realizamos la mayoría de la consulta en el lado del servidor
        //        var query = await context.TechnicalProfessionAvailabilityRequests
        //            .Include(x => x.Request)
        //                .ThenInclude(r => r.ImagesProblemRequests)
        //            .Include(x => x.TechnicalProfessionAvailability)
        //                .ThenInclude(tpa => tpa.Availability)
        //            .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId
        //                     && x.Request.StateRequestId == Constants.StateRequests.PENDING)
        //            .ToListAsync();



        //        // Pasamos la agrupación al lado del cliente
        //        var result = query
        //            .GroupBy(x => new
        //            {
        //                x.TechnicalProfessionAvailability,
        //                x.TechnicalProfessionAvailability.Availability
        //            }) // Agrupamos por TechnicalProfessionAvailability y Availability
        //            .Select(g => new TechnicalProfessionAvailabilityRequestResponse
        //            {
        //                TechnicalProfessionAvailability = new TechnicalProfessionAvailability
        //                {
        //                    Id = g.Key.TechnicalProfessionAvailability.Id,
        //                    TechnicalProfessionId = g.Key.TechnicalProfessionAvailability.TechnicalProfessionId,
        //                    AvailabilityId = g.Key.TechnicalProfessionAvailability.AvailabilityId,
        //                    Availability = g.Key.Availability,
        //                },
        //                Requests = g.Select(x => new Request
        //                {
        //                    Id = x.Request.Id,
        //                    StateRequestId = x.Request.StateRequestId,
        //                    CategoryRequestId = x.Request.CategoryRequestId,
        //                    ClientId = x.Request.ClientId,
        //                    TechnicalProfessionAvailabilityId = x.Request.TechnicalProfessionAvailabilityId,
        //                    ServiceTechnicalId = x.Request.ServiceTechnicalId,
        //                    Title = x.Request.Title,
        //                    Description = x.Request.Description,
        //                    Location = x.Request.Location,
        //                    ProposedAssistanceDate = x.Request.ProposedAssistanceDate,
        //                    AnsweredOn = x.Request.AnsweredOn,
        //                    ResolvedOn = x.Request.ResolvedOn,
        //                    FromShow = x.Request.FromShow,
        //                    ToShow = x.Request.ToShow,
        //                    IsResponse = x.Request.IsResponse,
        //                    ImagesProblemRequests = x.Request.ImagesProblemRequests,
        //                }).ToList()  // Las solicitudes asociadas a cada TechnicalProfessionAvailabilityId
        //            })
        //            .ToList();


        //        // Obtenemos las solicitudes directas
        //        var requests = await context.Requests
        //            .Include(r => r.TechnicalProfessionAvailability)
        //            .Where(x => x.TechnicalProfessionAvailabilityId != null &&
        //                        x.StateRequestId == Constants.StateRequests.PENDING &&
        //                        x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
        //            .ToListAsync();

        //        // Agregamos las solicitudes directas a la respuesta
        //        result.ForEach(tpar =>
        //        {        
        //            // Extraemos las solicitudes para cada disponibilidad
        //            tpar.Requests.AddRange(
        //                requests
        //                    .Where(r => r.TechnicalProfessionAvailability.AvailabilityId == tpar.TechnicalProfessionAvailability.AvailabilityId)
        //                    .ToList()
        //            );  
        //        });

        //        return result;
        //    }
        //}


        public async Task<List<TechnicalProfessionAvailabilityRequestResponse>> FindAllPendingByUserId(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {

                var result = new List<TechnicalProfessionAvailabilityRequestResponse>();

                var availabilities = await context.Availabilities.ToListAsync();

                // Agremos las disponibilidades y solicitudes directas
                foreach (var availability in availabilities)
                {
                    var TechProfRequest = new TechnicalProfessionAvailabilityRequestResponse
                    {
                        Availability = availability,
                        Requests = await context.Requests
                                    .Where(x => x.TechnicalProfessionAvailabilityId != null &&
                                                x.TechnicalProfessionAvailability.AvailabilityId == availability.Id &&
                                                x.StateRequestId == Constants.StateRequests.PENDING &&
                                                x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
                                    .ToListAsync()
                    };

                    // Agregar solcitiudes hecha a muchos agregada en bandeja
                    TechProfRequest.Requests.AddRange(
                        await context.TechnicalProfessionAvailabilityRequests
                            .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId &&
                                        x.Request.StateRequestId == Constants.StateRequests.PENDING &&
                                        x.TechnicalProfessionAvailability.AvailabilityId == availability.Id)
                            .Select(x => x.Request)
                            .ToListAsync()
                    );

                    result.Add(TechProfRequest);
                }

                return result;
            }
        }


    }
}
