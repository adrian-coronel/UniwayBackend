namespace UniwayBackend.Models.Payloads.Core.Request.PhotoUser
{
    public class PhotoUserRequest
    {
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
