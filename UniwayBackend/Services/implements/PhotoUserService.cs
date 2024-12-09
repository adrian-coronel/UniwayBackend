using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.PhotoUser;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class PhotoUserService : IPhotoUserService
    {
        private readonly IAws3Service _aws3Service;
        private readonly IPhotoUserRepository _repository;
        private readonly UtilitariesResponse<PhotoUser> _utilitaries;
        private readonly ILogger<PhotoUserService> _logger;

        public PhotoUserService(
            IAws3Service aws3Service,
            IPhotoUserRepository repository,
            UtilitariesResponse<PhotoUser> utilitaries,
            ILogger<PhotoUserService> logger)
        {
            _aws3Service = aws3Service;
            _repository = repository;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        public async Task<MessageResponse<PhotoUser>> Save(PhotoUserRequest photo)
        {
            MessageResponse<PhotoUser> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (photo.File == null || photo.File.Length == 0)
                    return _utilitaries.setResponseBaseForBadRequest("El archivo está vacío");

                var photoInBD = await _repository.FindByUserId(photo.UserId);

                FileResponse photoS3 = await _aws3Service.UploadFileAsync(photo.File);

                var photoSaveOrUpdate = new PhotoUser
                {
                    Id = photoInBD?.Id ?? 0,
                    Url = photoS3.Url,
                    OriginalName = photoS3.OriginalName,
                    ExtensionType = photoS3.ExtensionType,
                    ContentType = photoS3.ContentType,
                    UserId = photo.UserId,
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
