using AutoMapper;
using Azure.Core;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ServiceTechnicalService : IServiceTechnicalService
    {

        private readonly ILogger<ServiceTechnicalService> _logger;       
        private readonly UtilitariesResponse<ServiceTechnical> _utilitaries;
        private readonly IImagesServiceTechnicalService _imageServiceTechncialService;
        private readonly IServiceTechnicalRepository _repository;
        private readonly IStorageService _storageService; 
        private readonly IMapper _mapper;

        public ServiceTechnicalService(ILogger<ServiceTechnicalService> logger, UtilitariesResponse<ServiceTechnical> utilitaries, IImagesServiceTechnicalService imageServiceTechncialService, IServiceTechnicalRepository repository, IStorageService storageService, IMapper mapper)
        {
            _logger = logger;
            _utilitaries = utilitaries;
            _imageServiceTechncialService = imageServiceTechncialService;
            _repository = repository;
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<MessageResponse<ServiceTechnical>> GetById(int serviceTechnicalId)
        {
            MessageResponse<ServiceTechnical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var service = await _repository.GetByIdWithInformation(serviceTechnicalId);

                response = _utilitaries.setResponseBaseForObject(service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ServiceTechnical>> GetByTechnicaIdAndAvailabilityId(int technicalId, short availabilityId)
        {
            MessageResponse<ServiceTechnical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository
                                .FindByTechnicalIdAndAvailabilityId(technicalId, availabilityId);

                response = _utilitaries.setResponseBaseForList(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ServiceTechnical>> Save(ServiceTechnical serviceTechnical, List<IFormFile> Files)
        {
            MessageResponse<ServiceTechnical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Validations
                var validResult = ValidateImages(Files);
                if (validResult != null) return validResult;

                serviceTechnical = await _repository.InsertAndReturn(serviceTechnical);

                // Guardar imagenes
                if (Files.Count > 0)
                {
                    var imgResponse = await _imageServiceTechncialService.SaveAll(serviceTechnical.Id, Files);
                    serviceTechnical.Images = imgResponse.List!.ToList();
                }
                                                                                         
                response = _utilitaries.setResponseBaseForObject(serviceTechnical);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ServiceTechnical>> Update(ServiceTechnical serviceTechnical)
        {
            MessageResponse<ServiceTechnical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var service = await _repository.FindById(serviceTechnical.Id);

                if (service == null) return _utilitaries.setResponseBaseForNotFount();

                // Actualizamos las propiedades solo si los valores son válidos
                service.CategoryServiceId = serviceTechnical.CategoryServiceId != 0 ? serviceTechnical.CategoryServiceId : service.CategoryServiceId;
                service.TechnicalProfessionAvailabilityId = serviceTechnical.TechnicalProfessionAvailabilityId != 0 ? serviceTechnical.TechnicalProfessionAvailabilityId : service.TechnicalProfessionAvailabilityId;
                service.Name = !string.IsNullOrEmpty(serviceTechnical.Name) ? serviceTechnical.Name : service.Name;
                service.Description = !string.IsNullOrEmpty(serviceTechnical.Description) ? serviceTechnical.Description : service.Description;

                service = await _repository.UpdateAndReturn(service);

                response = _utilitaries.setResponseBaseForObject(service);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        private MessageResponse<ServiceTechnical>? ValidateImages(List<IFormFile> Files)
        {
            if (Files != null)
            {
                if (Files.Count > Constants.MAX_FILES)
                    return new MessageResponseBuilder<ServiceTechnical>()
                    .Code(400).Message($"La cantidad de archivos excede el limite(${Constants.MAX_FILES})").Build();

                if (Files.Any(x => !Constants.VALID_CONTENT_TYPES.Contains(x.ContentType)))
                    return new MessageResponseBuilder<ServiceTechnical>()
                    .Code(400).Message($"Imagenes con tipo de contenido no valido").Build();

                if (Files.Any(x => x.Length > (Constants.MAX_MB * 1024 * 1024)))
                    return new MessageResponseBuilder<ServiceTechnical>()
                    .Code(400).Message($"Uno de los archivos excedió el tamaño maximo de ${Constants.MAX_MB}MB").Build();
            }
            return null;
        }
    }
}
