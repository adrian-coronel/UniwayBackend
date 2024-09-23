using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniwayBackend.Services.implements;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _service;

        public StorageController(IStorageService service)
        {
            _service = service;
        }

        [HttpGet("{folder}/{fileName}")]
        public async Task<IActionResult> GetImage(string folder, string fileName)
        {
            var image = await _service.GetFileAsync(folder, fileName);

            if (image is null) return NotFound("Image no encontrada o su tipo no es valido");

            return File(image.FileContent, image.ContentType);
        }
    }
}
