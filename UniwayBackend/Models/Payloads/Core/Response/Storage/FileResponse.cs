namespace UniwayBackend.Models.Payloads.Core.Response.Storage
{
    public class FileResponse
    {
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public string FakeName { get; set; }
        public string FakeExtensionType { get; set; }
    }
}
