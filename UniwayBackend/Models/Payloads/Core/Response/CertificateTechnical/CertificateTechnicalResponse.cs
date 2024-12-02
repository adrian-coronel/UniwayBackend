namespace UniwayBackend.Models.Payloads.Core.Response.CertificateTechnical
{
    public class CertificateTechnicalResponse
    {
        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
