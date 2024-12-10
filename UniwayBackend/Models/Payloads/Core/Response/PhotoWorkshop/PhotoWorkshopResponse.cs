
namespace UniwayBackend.Models.Payloads.Core.Response.PhotoWorkshop
{
    public class PhotoWorkshopResponse
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
