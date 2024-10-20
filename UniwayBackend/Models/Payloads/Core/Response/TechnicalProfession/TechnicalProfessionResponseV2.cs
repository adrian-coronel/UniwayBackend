using UniwayBackend.Models.Payloads.Core.Response.Experience;
using UniwayBackend.Models.Payloads.Core.Response.Profession;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;
using UniwayBackend.Models.Payloads.Core.Response.UserTechnical;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession
{
    public class TechnicalProfessionResponseV2
    {
        public int Id { get; set; }
        public short ExperienceId { get; set; }
        public short ProfessionId { get; set; }
        public int UserTechnicalId { get; set; }

        public UserTechnicalResponseV2 UserTechnical{ get; set; }
        public ProfessionResponse Profession { get; set; }
        public ExperienceResponse Experience { get; set; }
    }
}
