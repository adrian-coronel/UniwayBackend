using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Client;

namespace UniwayBackend.Services.interfaces
{
    public interface IClientService
    {
        Task<MessageResponse<Client>> GetInformationByUser(Guid UserId);
        Task<MessageResponse<Client>> GetById(int Id);
    }
}
