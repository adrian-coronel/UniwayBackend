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
        private readonly IStorageService _storageService;
        private readonly UtilitariesResponse<ImagesProblemRequest> _utilitaries;

        public ImagesProblemRequestService(IImagesProblemRequestRepository repository,
                                           ILogger<ImagesProblemRequestService> logger,
                                           IStorageService storageService,
                                           UtilitariesResponse<ImagesProblemRequest> utilitaries)
        {
            _repository = repository;
            _logger = logger;
            _storageService = storageService;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<ImagesProblemRequest>> Save(int RequestId, IFormFile file)
        {
            MessageResponse<ImagesProblemRequest> response;
            DateTime currentDate = DateTime.UtcNow;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Guardamos las imagenes 
                ImageResponse image = await _storageService.SaveFileAsync(file, currentDate.ToString("yyyy-MM-dd"));

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
                List<ImageResponse> images = await _storageService.SaveFilesAsync(files, currentDate.ToString("yyyy-MM-dd"));

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
