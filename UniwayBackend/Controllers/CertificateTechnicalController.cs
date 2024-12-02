using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.CertificateTechnical;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTechnicalController : ControllerBase
    {

        private readonly ICertificateTechnicalRepository _certificateTechnicalRepository;
        private readonly ILogger<CertificateTechnicalController> _logger;
        private readonly IAws3Service _aws3Service;
        private readonly IMapper _mapper;

        public CertificateTechnicalController(ICertificateTechnicalRepository certificateTechnicalRepository, ILogger<CertificateTechnicalController> logger, IAws3Service aws3Service, IMapper mapper)
        {
            _certificateTechnicalRepository = certificateTechnicalRepository;
            _logger = logger;
            _aws3Service = aws3Service;
            _mapper = mapper;
        }

        [HttpGet("GetByTechnicalId/{technicalId}")]
        public async Task<ActionResult<MessageResponse<CertificateTechnicalResponse>>> GetByTechnicalId(int technicalId)
        {
            MessageResponse<CertificateTechnicalResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _certificateTechnicalRepository.GetByTechnicalId(technicalId);

                if (result == null) return new MessageResponseBuilder<CertificateTechnicalResponse>()
                    .Code(404).Message("No se encontro el recurso solicitado").Build();

                var mapped = _mapper.Map<CertificateTechnical, CertificateTechnicalResponse>(result);

                response = new MessageResponseBuilder<CertificateTechnicalResponse>()
                    .Code(200).Object(mapped).Build();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<CertificateTechnicalResponse>()
                    .Code(500).Message(ex.Message).Build();
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet("GetFile/{filename}")]
        public async Task<IActionResult> GetFile(string filename)
        {
            try
            {

                var certificateTechnical = await _certificateTechnicalRepository.GetByFileName(filename);

                if (certificateTechnical == null) return NotFound();

                // Separar el nombre del archivo y la carpeta desde fileRequest.Path
                var filePath = certificateTechnical?.Url;
                var folder = Path.GetDirectoryName(filePath);
                var fileName = Path.GetFileName(filePath);

                var file = await _aws3Service.DownloadFileAsync(filePath);
                
                if (file == null)
                    return NotFound("El archivo solicitado no existe.");

                // Usar los bytes del archivo para devolverlo al cliente
                return File(file, certificateTechnical!.ContentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el archivo: {ex.Message}");
            }
        }
    }
}
