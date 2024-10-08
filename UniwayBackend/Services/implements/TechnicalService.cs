using AutoMapper;
using NetTopologySuite.Geometries;
using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Technical;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class TechnicalService : ITechnicalService
    {

        private readonly ILogger<TechnicalService> _logger;
        private readonly ITechnicalRepository _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly UtilitariesResponse<Technical> _utilitaries;

        public TechnicalService(ILogger<TechnicalService> logger,
                                ITechnicalRepository repository,
                                IMapper mapper,
                                INotificationService notificationService,
                                UtilitariesResponse<Technical> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Technical>> GetInformation(int TechnicalId)
        {
            MessageResponse<Technical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _repository.FindTechnicalWithInformation(TechnicalId);

                response = _utilitaries.setResponseBaseForObject(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Technical>> UpdateWorkinStatus(TechnicalRequestV1 request)
        {
            MessageResponse<Technical> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                bool existTechnical = await _repository.ExistById(request.TechnicalId);

                if (!existTechnical) return _utilitaries.setResponseBaseNotFoundForUpdate();

                List<UserRequest> userRequests = await _repository.UpdateWorkingStatus(request.TechnicalId,request.WorkingStatus,request.Lat,request.Lng, request.Distance.Value);

                // Si se encontrarón solicitudes para el técnico notificarlas
                foreach (var userRequest in userRequests)
                {

                    RequestResponse requestMap = _mapper.Map<RequestResponse>(userRequest);

                    await _notificationService.SendNotificationWithRequestAsync(
                        userRequest.UserId.ToString(),
                        new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                            Message = "Notification success",
                            Data = requestMap,
                        }
                    );
                }

                response = _utilitaries.setResponseBaseForOk();
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
