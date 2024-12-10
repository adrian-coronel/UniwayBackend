using UniwayBackend.Models.Payloads.Core.Response.CertificateTechnical;
using UniwayBackend.Models.Payloads.Core.Response.Review;
using UniwayBackend.Models.Payloads.Core.Response.UserTechnical;

namespace UniwayBackend.Models.Payloads.Core.Response.Technical
{
    public class TechnicalResponseV2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherLastname { get; set; }
        public string MotherLastname { get; set; }
        public string Dni { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool WorkingStatus { get; set; }
        public required bool Enabled { get; set; }
        public virtual List<ReviewResponseV2> Reviews { get; set; }
        public virtual CertificateTechnicalResponse CertificateTechnical { get; set; }
        public virtual List<UserTechnicalResponse> UserTechnicals { get; set; }
    }
}
