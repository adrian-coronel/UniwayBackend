
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest
{
    public class TechnicalProfessionAvailabilityRequestResponseV2
    {
        public TechnicalProfessionAvailabilityResponse TechnicalProfessionAvailability { get; set; }
        public List<RequestResponse> Requests { get; set; }
    }
}
