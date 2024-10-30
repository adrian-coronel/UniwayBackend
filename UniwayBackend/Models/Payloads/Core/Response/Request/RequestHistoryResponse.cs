using UniwayBackend.Models.Payloads.Core.Response.StateRequest;

namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestHistoryResponse
    {
        public string Month { get; set; }
        public List<RequestsForStateResponse> StateResponses { get; set; }
    }
}
