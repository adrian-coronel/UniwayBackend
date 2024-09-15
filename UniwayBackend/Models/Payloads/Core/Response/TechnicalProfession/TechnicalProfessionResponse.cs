using System.ComponentModel.DataAnnotations.Schema;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response.Experience;
using UniwayBackend.Models.Payloads.Core.Response.Profession;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession
{
    public class TechnicalProfessionResponse
    {
        public int Id { get; set; }
        public short ExperienceId { get; set; }
        public short ProfessionId { get; set; }
        public int UserTechnicalId { get; set; }

        public ProfessionResponse Profession { get; set; }
        public ExperienceResponse Experience { get; set; }
        public virtual List<TechnicalProfessionAvailabilityResponse> TechnicalProfessionAvailabilities { get; set; }
    }
}
