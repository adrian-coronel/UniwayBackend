
using UniwayBackend.Models.Payloads.Core.Response.Storage;

namespace UniwayBackend.Services.interfaces
{
    public interface IStorageService
    {
        Task<ImageGetResponse?> GetFileAsync(string folder, string fileName);
        Task<ImageResponse> SaveFileAsync(IFormFile file, string folder);
        Task<List<ImageResponse>> SaveFilesAsync(List<IFormFile> files, string folder); // Nueva función para guardar múltiples archivos
        Task DeleteFileAsync(string filePath, string folder);
      
    }
}
