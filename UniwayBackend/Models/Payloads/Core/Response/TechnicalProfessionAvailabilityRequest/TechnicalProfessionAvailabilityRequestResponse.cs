
namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest
{
    public class TechnicalProfessionAvailabilityRequestResponse
    {
        public Entities.TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }
        public List<Entities.Request> Requests { get; set; }
    }
}
