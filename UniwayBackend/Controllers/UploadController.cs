using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Subir un archivo
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado un archivo válido.");

            try
            {
                var folder = "Services\\" + DateTime.Now.ToString("yyyy-MM-dd");

                var response = await _storageService.SaveFileAsync(file, folder);

                return Ok(new { Message = "Archivo subido exitosamente", FileDetails = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir el archivo: {ex.Message}");
            }
        }

        /// <summary>
        /// Subir múltiples archivos
        /// </summary>
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultipleFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No se han proporcionado archivos.");

            try
            {
                var folder = "Services\\" + DateTime.Now.ToString("yyyy-MM-dd");

                var responses = await _storageService.SaveFilesAsync(files, folder);
                return Ok(new { Message = "Archivos subidos exitosamente", FilesDetails = responses });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir los archivos: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtener un archivo
        /// </summary>
        //[HttpPost("GetFile")]
        //public async Task<IActionResult> GetFile([FromBody] FileRequest fileRequest)
        //{
        //    try
        //    {
        //        if (fileRequest == null || string.IsNullOrWhiteSpace(fileRequest.Path))
        //            return BadRequest("Se requiere una solicitud válida con la ruta del archivo.");

        //        // Separar el nombre del archivo y la carpeta desde fileRequest.Path
        //        var filePath = fileRequest.Path;
        //        var folder = Path.GetDirectoryName(filePath);
        //        var fileName = Path.GetFileName(filePath);

        //        var file = await _storageService.GetFileAsync(folder, fileName);
        //        if (file == null)
        //            return NotFound("El archivo solicitado no existe.");

        //        return File(file.Stream, file.ContentType, fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al obtener el archivo: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Eliminar un archivo
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string filePath, [FromQuery] string folder)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return BadRequest("Se requiere la ruta del archivo para eliminar.");

            try
            {
                await _storageService.DeleteFileAsync(filePath, folder);
                return Ok(new { Message = "Archivo eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el archivo: {ex.Message}");
            }
        }
    }
}
