using System.Security.Cryptography;
using System.Text;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class StorageService : IStorageService
    { 
        private readonly string _storageRoot;

        public StorageService(IConfiguration configuration)
        { 
            _storageRoot = configuration["Storage:RootPath"] ?? "Uploads";
        }

        // Función para guardar un solo archivo
        public async Task<ImageResponse> SaveFileAsync(IFormFile file, string folder)
        {
            // Crear la carpeta si no existe
            var folderPath = Path.Combine(_storageRoot, folder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Crear nombre hasheado y cambiar la extensión a .wtz
            var uniqueFileName = GenerateHashedFileName(Path.GetFileNameWithoutExtension(file.FileName), ".wtz");
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            // Guardar el archivo
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //return Path.Combine(folder, uniqueFileName);
            return new ImageResponse
            {
                Url = Path.Combine(folder, uniqueFileName),
                OriginalName = Path.GetFileNameWithoutExtension(file.FileName),
                ExtensionType = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                FakeName = Path.GetFileNameWithoutExtension(uniqueFileName),
                FakeExtensionType = Path.GetExtension(uniqueFileName)
            };
        }

        // Nueva función para guardar múltiples archivos
        public async Task<List<ImageResponse>> SaveFilesAsync(List<IFormFile> files, string folder)
        {
            List<ImageResponse> filePaths = new List<ImageResponse>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = await SaveFileAsync(file, folder); // Reutilizamos la función de guardado individual
                    filePaths.Add(filePath);
                }
            }

            return filePaths;
        }

        public async Task DeleteFileAsync(string filePath, string folder)
        {
            var fullPath = Path.Combine(_storageRoot, folder, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            await Task.CompletedTask;
        }

        // Método para generar un nombre hasheado para los archivos
        private string GenerateHashedFileName(string originalFileName, string extension)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalFileName + DateTime.Now.Ticks));
                var hashedFileName = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return hashedFileName + extension; // Cambiar la extensión a .wtz
            }
        }

    }
}
