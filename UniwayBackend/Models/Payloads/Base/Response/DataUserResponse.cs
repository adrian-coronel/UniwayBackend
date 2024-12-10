namespace UniwayBackend.Models.Payloads.Base.Response
{
    public class DataUserResponse
    {
        public string EntityId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string TypeEntity { get; set; }
        public string? TechnicalId { get; set; }
    }
}
