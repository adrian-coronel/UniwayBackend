namespace UniwayBackend.Models.Payloads.Core.Response.ImageProblem
{
    public class ImagesProblemRequestResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
