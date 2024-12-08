﻿using AutoMapper;
using Azure.Core;
using System.Reflection;
using System.Runtime.InteropServices;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Workshop;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.ImageProblem;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;
using NetTopologySuite.Geometries;

namespace UniwayBackend.Services.implements
{
    public class WorkshopService : IWorkshopService
    {

        private readonly ILogger<WorkshopService> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkshopRepository _repository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IImagesProblemRequestRepository _imagesProblemRepository;
        private readonly UtilitariesResponse<Workshop> _utilitaries;

        public WorkshopService(ILogger<WorkshopService> logger,
                               IMapper mapper,
                               IWorkshopRepository repository,
                               IClientRepository clientRepository,
                               IUserRepository userRepository,
                               INotificationService notificationService,
                               IImagesProblemRequestRepository imagesProblemRepository,
                               UtilitariesResponse<Workshop> utilitaries)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _imagesProblemRepository = imagesProblemRepository;
            _utilitaries = utilitaries;
        }

        public async Task<MessageResponse<Workshop>> Save(Workshop workshop)
        {
            MessageResponse<Workshop> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (workshop.Id > 0) return _utilitaries.setResponseBaseForBadRequest();
                workshop.Location = new Point(workshop.Lng.Value, workshop.Lat.Value) { SRID = 4326 };
                workshop = await _repository.InsertAndReturn(workshop);

                response = _utilitaries.setResponseBaseForObject(workshop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Workshop>> Update(Workshop workshop)
        {
            MessageResponse<Workshop> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (workshop.Id <= 0) return _utilitaries.setResponseBaseForBadRequest();

                Workshop? WorkshopFind = await _repository.FindById(workshop.Id);

                if (WorkshopFind == null) return _utilitaries.setResponseBaseNotFoundForUpdate();

                WorkshopFind.Name = string.IsNullOrEmpty(workshop.Name) ? WorkshopFind.Name : workshop.Name;
                WorkshopFind.Location = workshop.Location == null ? WorkshopFind.Location : workshop.Location;
                WorkshopFind.Description = workshop.Description;
                WorkshopFind.WorkingStatus = workshop.WorkingStatus;

                workshop = await _repository.UpdateAndReturn(WorkshopFind);

                response = _utilitaries.setResponseBaseForObject(WorkshopFind);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
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
                    Client? client = await _clientRepository.FindById(userRequest.ClientId);
                    RequestResponse requestMap = _mapper.Map<RequestResponse>(userRequest);

                    // Bsucar las imagenes de la solicitud
                    List<ImagesProblemRequest> images = await _imagesProblemRepository.FindAllByRequestId(requestMap.Id);
                    requestMap.ImagesProblemRequests = _mapper.Map<List<ImagesProblemRequestResponse>>(images);

                    await _notificationService.SendNotificationWithRequestAsync(
                        userRequest.UserId.ToString(),
                        new NotificationResponse
                        {
                            Type = Constants.TypesConnectionSignalR.SOLICITUDE,
                            Message = "Notification success",
                            Data = requestMap,
                            UserSend = new DataUserResponse
                            {
                                EntityId = client.Id.ToString(),
                                FullName = $"{client.Name} {client.FatherLastname} {client.MotherLastname}",
                                PhoneNumber = client.PhoneNumber,
                                TypeEntity = Constants.EntityTypes.CLIENT
                            }
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
