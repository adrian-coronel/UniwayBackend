namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestForWeekend
    {
        public string Weekend { get; set; }
        public List<RequestsForStateResponse> StatesRequest { get; set; }
    }
}
