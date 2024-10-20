using UniwayBackend.Models.Payloads.Core.Response.Technical;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession;
using UniwayBackend.Models.Payloads.Core.Response.TowingCar;

namespace UniwayBackend.Models.Payloads.Core.Response.UserTechnical
{
    public class UserTechnicalResponseV2
    {
        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public Guid UserId { get; set; }
        public bool Enabled { get; set; }

        public virtual TechnicalResponseV1 Technical { get; set; }
        public virtual UserResponse User { get; set; }
    }
}
