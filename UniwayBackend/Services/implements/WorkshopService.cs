using System.Reflection;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Workshop;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class WorkshopService : IWorkshopService
    {

        private readonly ILogger<WorkshopService> _logger;
        private readonly IWorkshopRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly UtilitariesResponse<Workshop> _utilitaries;

        public WorkshopService(ILogger<WorkshopService> logger,
                               IWorkshopRepository repository,
                               INotificationService notificationService,
                               UtilitariesResponse<Workshop> utilitaries)
        {
            _logger = logger;
            _repository = repository;
            _notificationService = notificationService;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Workshop>> UpdateWorkshopStatus(WorkshopRequestV1 request)
        {
            MessageResponse<Workshop> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var workshop = await _repository.ExistById(request.WorkshopId);
                if (workshop == null) return _utilitaries.setResponseBaseNotFoundForUpdate();

                List<UserRequest> userRequests = await _repository.UpdateWorkingStatus(request.WorkshopId, request.WorkingStatus, request.Lat, request.Lng, request.Distance.Value);

                // Si se encontrarón solicitudes para el técnico notificarlas
                foreach (var userRequest in userRequests)
                {
                    var requestEntity = new Request
                    {
                        Id = userRequest.RequestId,
                        StateRequestId = userRequest.StateRequestId,
                        CategoryRequestId = userRequest.CategoryRequestId,
                        ClientId = userRequest.ClientId,
                        TechnicalProfessionAvailabilityId = userRequest.TechnicalProfessionAvailabilityId,
                        ServiceTechnicalId = userRequest.ServiceTechnicalId,
                        Title = userRequest.Title,
                        Description = userRequest.Description,
                        Location = userRequest.Location,
                        ProposedAssistanceDate = userRequest.ProposedAssistanceDate,
                        AnsweredOn = userRequest.AnsweredOn,
                        ResolvedOn = userRequest.ResolvedOn,
                        FromShow = userRequest.FromShow,
                        ToShow = userRequest.ToShow,
                        IsResponse = userRequest.IsResponse
                    };

                    await _notificationService.SendNotificationWithRequestAsync(
                        userRequest.UserId.ToString(),
                        new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                            Message = "Notification success",
                            Data = requestEntity,
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
