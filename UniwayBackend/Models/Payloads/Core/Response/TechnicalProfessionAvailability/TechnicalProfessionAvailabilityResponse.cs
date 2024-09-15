using System.ComponentModel.DataAnnotations.Schema;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response.Availability;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability
{
    public class TechnicalProfessionAvailabilityResponse
    {
        public int Id { get; set; }
        public short AvailabilityId { get; set; }
        public int TechnicalProfessionId { get; set; }

        public virtual AvailabilityResponse Availability { get; set; }
        public virtual List<WorkshopResponse> Workshops { get; set; } 
    }
}
