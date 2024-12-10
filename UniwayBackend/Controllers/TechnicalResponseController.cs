using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.TechnicalResponse;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalResponse;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalResponseController : ControllerBase
    {
        private readonly ILogger<TechnicalResponseController> _logger;
        private readonly ITechnicalResponseService _service;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TechnicalResponseController(ILogger<TechnicalResponseController> logger, ITechnicalResponseService service, INotificationService notificationService, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetByRequestId/{RequestId}")]
        public async Task<ActionResult<MessageResponse<TechnicalResponseResponseV2>>> GetByRequestId(int RequestId)
        {
            MessageResponse<TechnicalResponseResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllByRequestId(RequestId);

                response = _mapper.Map<MessageResponse<TechnicalResponseResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new MessageResponseBuilder<TechnicalResponseResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpGet("GetByClientAndRequest/{ClientId}/{RequestId}")]
        public async Task<ActionResult<MessageResponse<TechnicalResponseResponseV2>>> GetByClientAndRequest(int ClientId, int RequestId)
        {
            MessageResponse<TechnicalResponseResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var result = await _service.GetAllByClientIdAndRequestId(ClientId, RequestId);

                response = _mapper.Map<MessageResponse<TechnicalResponseResponseV2>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new MessageResponseBuilder<TechnicalResponseResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse<TechnicalResponseResponseV2>>> Save(TechnicalResponseRequest request)
        {
            MessageResponse<TechnicalResponseResponseV2> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                // Mapear a entidad
                var resultMapped = _mapper.Map<TechnicalResponse>(request);

                // Guardar la entidad
                var entity = await _service.Save(resultMapped);

                // Mapear a dto response
                response = _mapper.Map<MessageResponse<TechnicalResponseResponseV2>>(entity);

                if (response.Code == 200) 
                {
                    User? user = await _userRepository.FindByRequestId(response.Object!.RequestId);
                    DataUserResponse userSend = await _userRepository.FindTechnicalOrWorkshop(request.TechnicalProfessionAvailabilityId.Value);

                    // Enviar la notificacion al cliente
                    if (user != null)
                        await _notificationService.SendNotificationWithTechnicalResponseAsync(user!.Id.ToString(), new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.RESPONSE,
                            TypeAttentionRequest = request.TypeAttentionRequest,
                            Message = "Notification success",
                            Data = response.Object,
                            UserSend = userSend,
                            ImageUser = user.PhotoUser?.Url ?? "default_image.png"
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new MessageResponseBuilder<TechnicalResponseResponseV2>()
                    .Code(500).Message(ex.Message).Build();
            }
            return response;
        }

    }
}
