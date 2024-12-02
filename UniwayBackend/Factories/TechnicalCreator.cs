using Azure.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Exceptions;
using UniwayBackend.Helpers;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.implements;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Factories
{
    public class TechnicalCreator : IUser
    {
        private readonly int RoleId = Constants.Roles.TECHNICAL_ID;

        private readonly ILogger<TechnicalCreator> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IUserTechnicalRepository _userTechnicalRepository;
        private readonly ICertificateTechnicalRepository _certificateTechnicalRepository;
        //private readonly IStorageService _storageService;
        private readonly IAws3Service _aws3Service;

        public TechnicalCreator(ILoggerFactory loggerFactory, IConfiguration configuration, AwsCredentialsManager credentialsManager)
        {
            _logger = loggerFactory.CreateLogger<TechnicalCreator>();
            _userRepository = new UserRepository();
            _technicalRepository = new TechnicalRepository();
            _userTechnicalRepository = new UserTechnicalRepository();
            _certificateTechnicalRepository = new CertificateTechnicalRepository();
            //_storageService = new StorageService(configuration);
            _aws3Service = new Aws3Service(credentialsManager);
        }


        public int GetRoleId()
        {
            return this.RoleId;
        }

        public async Task<User> Create(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                
                if (request.File == null) throw new BadHttpRequestException("El archivo PDF es requerido para el mecánico");

                // Creación del User
                User user = new User
                {
                    Email = request.Email,
                    Password = request.Password,
                    RoleId = request.RoleId,
                    Enabled = Constants.State.ACTIVE_BOOL,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };

                user = await _userRepository.InsertAndReturn(user);

                // Creación del Technical
                Technical technical = new Technical
                {
                    Name = request.Name,
                    FatherLastname = request.FatherLastname,
                    MotherLastname = request.MotherLastname,
                    Dni = request.Dni,
                    PhoneNumber = request.PhoneNumber,
                    BirthDate = request.BirthDate,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                technical = await _technicalRepository.InsertAndReturn(technical);

                UserTechnical userTechnical = new UserTechnical
                {
                    UserId = user.Id,
                    TechnicalId = technical.Id,
                    Enabled = Constants.State.ACTIVE_BOOL
                };

                await _userTechnicalRepository.Insert(userTechnical);

                var folder = "certificates\\" + DateTime.Now.ToString("yyyy-MM-dd");

                // Implementar logica de S3
                var result = await _aws3Service.UploadFileAsync(request.File);

                await _certificateTechnicalRepository.Insert(new CertificateTechnical
                {
                    Id = 0,
                    TechnicalId = technical.Id,
                    Url = result.Url,
                    OriginalName = result.OriginalName,
                    ExtensionType = result.ExtensionType,
                    ContentType = result.ContentType,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = null
                });

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public async Task<User> Edit(ProfileRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                UserTechnical? userTechnical = await _userTechnicalRepository.FindByUserIdAndRoleId(request.UserId, request.RoleId);

                if (userTechnical is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {request.UserId} e ID de rol {request.RoleId}");

                userTechnical.User.Password = !string.IsNullOrEmpty(request.Password) ? request.Password : userTechnical.User.Password;
                userTechnical.Technical.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : userTechnical.Technical.Name;
                userTechnical.Technical.FatherLastname = !string.IsNullOrEmpty(request.FatherLastname) ? request.FatherLastname : userTechnical.Technical.FatherLastname;
                userTechnical.Technical.MotherLastname = !string.IsNullOrEmpty(request.MotherLastname) ? request.MotherLastname : userTechnical.Technical.MotherLastname;
                userTechnical.Technical.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : userTechnical.Technical.PhoneNumber;
                userTechnical.Technical.BirthDate = request.BirthDate.HasValue ? request.BirthDate.Value : userTechnical.Technical.BirthDate;

                userTechnical = await _userTechnicalRepository.UpdateAndReturn(userTechnical);

                return userTechnical.User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<User> Delete(Guid UserId)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                UserTechnical? userTechnical = await _userTechnicalRepository.FindByUserIdAndRoleId(UserId, GetRoleId());

                if (userTechnical is null)
                    throw new NotFoundException($"No se encontró el usuario con ID {UserId} e ID de rol {GetRoleId()}");

                userTechnical.User.Enabled = false;
                userTechnical.Enabled = false;

                userTechnical = await _userTechnicalRepository.UpdateAndReturn(userTechnical);

                return userTechnical.User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }
}
