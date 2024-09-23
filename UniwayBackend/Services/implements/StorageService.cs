using System.Security.Cryptography;
using System.Text;
using UniwayBackend.Config;
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

        public async Task<ImageGetResponse?> GetFileAsync(string folder, string fileName)
        {
            var filePath = Path.Combine(_storageRoot, folder, fileName);
            var contentType = GetContentType(Path.GetExtension(fileName));

            // Si el archivo no existe o el contenido no es valido
            if (!File.Exists(filePath) || contentType == null) return null;

            var fileContents = await File.ReadAllBytesAsync(filePath);
            
            return new ImageGetResponse
            {
                FileContent = fileContents,
                ContentType = contentType,
                FileName = fileName
            };
        }

        private string? GetContentType(string fileExtension)
        {
            return Constants.VALID_TYPES.ContainsKey(fileExtension)
                ? Constants.VALID_TYPES[fileExtension]
                : null;
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

            // Generar un nombre de archivo único
            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}";
            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = $"{fileName}{fileExtension}";

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
                OriginalName = fileName,
                ExtensionType = fileExtension,
                ContentType = file.ContentType,
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

        

    }
}
