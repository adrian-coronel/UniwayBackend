namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class PhotoWorkshopRequest
    {
        public int WorkshopId { get; set; }
        public IFormFile File { get; set; }
    }
}
