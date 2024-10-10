
using UniwayBackend.Models.Payloads.Core.Response.Availability;
using UniwayBackend.Models.Payloads.Core.Response.Request;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest
{
    public class TechnicalProfessionAvailabilityRequestResponseV2
    {
        public AvailabilityResponse Availability { get; set; }
        public List<RequestResponse> Requests { get; set; }
    }
}
