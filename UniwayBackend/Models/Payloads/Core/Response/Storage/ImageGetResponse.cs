namespace UniwayBackend.Models.Payloads.Core.Response.Storage
{
    public class ImageGetResponse
    {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
