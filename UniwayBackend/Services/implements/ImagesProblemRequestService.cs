using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ImagesProblemRequestService : IImagesProblemRequestService
    {
        private readonly IImagesProblemRequestRepository _repository;
        private readonly ILogger<ImagesProblemRequestService> _logger;
        private readonly UtilitariesResponse<ImagesProblemRequest> _utilitaries;
        private readonly IAws3Service _aws3Service;

        public ImagesProblemRequestService(IImagesProblemRequestRepository repository,
                                           ILogger<ImagesProblemRequestService> logger,
                                           UtilitariesResponse<ImagesProblemRequest> utilitaries,
                                           IAws3Service aws3Service)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
            _aws3Service = aws3Service;
        }

        public async Task<MessageResponse<ImagesProblemRequest>> Save(int RequestId, IFormFile file)
        {
            MessageResponse<ImagesProblemRequest> response;
            DateTime currentDate = DateTime.UtcNow;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Guardamos las imagenes 
                var image = await _aws3Service.UploadFileAsync(file);

                // Guardamos los datos y ubicación de las imgenes en BD
                ImagesProblemRequest imagesProblemMapped = new ImagesProblemRequest
                {
                    RequestId = RequestId,
                    Url = image.Url,
                    OriginalName = image.OriginalName,
                    ExtensionType = image.ExtensionType,
                    ContentType = image.ContentType,
                    CreatedOn = DateTime.UtcNow,
                };

                var result = await _repository.InsertAndReturn(imagesProblemMapped);

                response = _utilitaries.setResponseBaseForObject(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ImagesProblemRequest>> SaveAll(int RequestId, List<IFormFile> files)
        {
            MessageResponse<ImagesProblemRequest> response;
            DateTime currentDate = DateTime.UtcNow;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                //if (imagesProblemRequests.Any(x => x.Id != 0)) return _utilitaries.setResponseBaseForInternalServerError();

                // Guardamos las imagenes 
                var images = await _aws3Service.UploadFilesAsync(files);

                // Guardamos los datos y ubicación de las imgenes en BD
                List<ImagesProblemRequest> imagesProblemMapped = images.Select(x => new ImagesProblemRequest
                {
                    RequestId = RequestId,
                    Url = x.Url,
                    OriginalName = x.OriginalName,
                    ExtensionType = x.ExtensionType,
                    ContentType = x.ContentType,
                    CreatedOn = DateTime.UtcNow,
                }).ToList();

                var result = await _repository.InsertAll(imagesProblemMapped);

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
