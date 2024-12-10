namespace UniwayBackend.Models.Payloads.Core.Request.PhotoUser
{
    public class PhotoWorkshopRequest
    {
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
