using UniwayBackend.Models.Payloads.Core.Response.StateRequest;

namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestHistoryResponse
    {
        public DateTime Month { get; set; }
        public List<RequestsForStateResponse> StateRequests { get; set; }
    }
}
