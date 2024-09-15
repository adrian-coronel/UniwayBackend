using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession;
using UniwayBackend.Models.Payloads.Core.Response.TowingCar;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Models.Payloads.Core.Response.UserTechnical
{
    public class UserTechnicalResponse
    {
        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public Guid UserId { get; set; }
        public bool Enabled { get; set; }
        public virtual UserResponse User { get; set; }
        public virtual List<TechnicalProfessionResponse> TechnicalProfessions { get; set; }
        public virtual List<TowingCarResponse> TowingCars { get; set; }
    }
}
