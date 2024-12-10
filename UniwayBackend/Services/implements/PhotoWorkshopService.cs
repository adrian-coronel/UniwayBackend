using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class PhotoWorkshopService : IPhotoWorkshopService
    {
        private readonly IAws3Service _aws3Service;
        private readonly IPhotoWorkshopRepository _repository;
        private readonly UtilitariesResponse<PhotoWorkshop> _utilitaries;
        private readonly ILogger<PhotoWorkshopService> _logger;

        public PhotoWorkshopService(IAws3Service aws3Service, IPhotoWorkshopRepository repository, UtilitariesResponse<PhotoWorkshop> utilitaries, ILogger<PhotoWorkshopService> logger)
        {
            _aws3Service = aws3Service;
            _repository = repository;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        public async Task<MessageResponse<PhotoWorkshop>> Save(PhotoWorkshopRequest photo)
        {
            MessageResponse<PhotoWorkshop> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (photo.File == null || photo.File.Length == 0)
                    return _utilitaries.setResponseBaseForBadRequest("El archivo está vacío");

                var photoInBD = await _repository.FindByWorkshopId(photo.WorkshopId);

                FileResponse photoS3 = await _aws3Service.UploadFileAsync(photo.File);

                var photoSaveOrUpdate = new PhotoWorkshop
                {
                    Id = photoInBD?.Id ?? 0,
                    Url = photoS3.Url,
                    OriginalName = photoS3.OriginalName,
                    ExtensionType = photoS3.ExtensionType,
                    ContentType = photoS3.ContentType,
                    WorkshopId = photo.WorkshopId,
                    CreatedOn = photoInBD?.CreatedOn ?? DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                if (photoInBD != null)
                {
                    await _aws3Service.DeleteFileAsync(photoInBD.Url);
                    await _repository.UpdateAndReturn(photoSaveOrUpdate);
                }
                else
                {
                    await _repository.InsertAndReturn(photoSaveOrUpdate);
                }

                response = _utilitaries.setResponseBaseForObject(photoSaveOrUpdate);
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
