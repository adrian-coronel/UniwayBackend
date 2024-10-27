using UniwayBackend.Models.Payloads.Core.Response.StateRequest;

namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestsForStateResponse
    {
        public StateRequestResponse StateRequest { get; set; }
        public List<RequestResponse> Requests { get; set; }
    }
}
