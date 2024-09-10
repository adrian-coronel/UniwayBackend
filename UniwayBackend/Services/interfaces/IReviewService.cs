using System.Runtime.ConstrainedExecution;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IReviewService
    {
        //Debe tener la opción de ver reseñas del mecánico mediante un boton
        Task<MessageResponse<Review>> GetAllByTechnical(int TechnicalId);
        //Debe poder ver la cantidad de estrellas y porcentajes en base a estadisticas en base a clausulas de atención al cliente.
        Task<MessageResponse<ReviewSummaryResponse>> GetSummaryByTechnical(int TechnicalId);
    }
}
