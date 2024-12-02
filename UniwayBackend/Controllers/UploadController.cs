using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Request.File;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IAws3Service _aws3Service;

        public UploadController(IAws3Service aws3Service)
        {
            _aws3Service = aws3Service;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            try
            {
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    return NotFound("Es requerida la extensión");

                if (!Constants.VALID_TYPES.ContainsKey(extension))
                    return NotFound("Extensión invalida");

                var file = await _aws3Service.DownloadFileAsync(fileName);

                if (!file.Any()) 
                    return NotFound("Archivo no encontrado");

                return File(file, Constants.VALID_TYPES.GetValueOrDefault(extension)!);
            }
            catch (Exception)
            {
                return NotFound("Archivo no encontrado");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileRequest request)
        {
            try
            {
                var extension = Path.GetExtension(request.File.FileName);

                if (request.File.Length == 0)
                    return NotFound("El archivo no tiene contenido");

                if (!Constants.VALID_TYPES.ContainsKey(extension))
                    return NotFound("Extensión invalida");

                var response = await _aws3Service.UploadFileAsync(request.File);

                return Ok(new { Message = "Archivo subido exitosamente", FileDetails = response }); ;
            }
            catch (Exception)
            {
                return BadRequest("El archivo no se pudo subir correctamente");
            }
        }
    }
}
