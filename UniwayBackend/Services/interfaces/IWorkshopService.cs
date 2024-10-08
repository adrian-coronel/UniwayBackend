﻿using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Workshop;

namespace UniwayBackend.Services.interfaces
{
    public interface IWorkshopService
    {
        Task<MessageResponse<Workshop>> UpdateWorkshopStatus(WorkshopRequestV1 request);
    }
}