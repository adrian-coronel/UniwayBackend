using UniwayBackend.Models.Payloads.Core.Response.Storage;

namespace UniwayBackend.Services.interfaces
{
    public interface IAws3Service
    {
        Task<byte[]> DownloadFileAsync(string file);
        Task<List<FileResponse>> UploadFilesAsync(IEnumerable<IFormFile> files);
        Task<FileResponse> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileName, string versionId = "");
    }
}
