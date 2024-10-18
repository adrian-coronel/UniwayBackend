namespace UniwayBackend.Models.Payloads.Core.Response.ImagesServiceTechnical
{
    public class ImagesServiceTechnicalResponse
    {
        public int Id { get; set; }
        public int ServiceTechnicalId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
