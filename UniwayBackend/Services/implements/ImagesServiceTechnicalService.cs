using Azure.Core;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Services.interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace UniwayBackend.Services.implements
{
    public class ImagesServiceTechnicalService : IImagesServiceTechnicalService
    {
        private readonly IBaseRepository<ImagesServiceTechnical, int> _repository;
        //private readonly IStorageService _storageService;
        private readonly ILogger<ImagesServiceTechnicalService> _logger;
        private readonly UtilitariesResponse<ImagesServiceTechnical> _utilitaries;
        private readonly IAws3Service _aws3Service;

        public ImagesServiceTechnicalService(
            IBaseRepository<ImagesServiceTechnical, int> repository,
            ILogger<ImagesServiceTechnicalService> logger,
            UtilitariesResponse<ImagesServiceTechnical> utilitaries,
            IAws3Service aws3Service)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
            _aws3Service = aws3Service;
        }

        public async Task<MessageResponse<ImagesServiceTechnical>> SaveAll(int ServiceTechnicalId, List<IFormFile> Files)
        {
            MessageResponse<ImagesServiceTechnical> response;
            DateTime currentDate = DateTime.UtcNow;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Guardamos las imagenes 
                var images = await _aws3Service.UploadFilesAsync(Files);

                // Guardamos los datos y ubicación de las imgenes en BD
                var ImagesServiceTechnicals = images.Select(image => new ImagesServiceTechnical
                {
                    ServiceTechnicalId = ServiceTechnicalId,
                    Url = image.Url,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.Now
                }).ToList();

                var result = await _repository.InsertAll(ImagesServiceTechnicals);

                response = _utilitaries.setResponseBaseForList(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
