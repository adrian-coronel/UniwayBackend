using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class ProfileRequest
    {
        // User credentials
        public string? Password { get; set; }

        // Atributos cliente, tecnico
        public string? Name { get; set; }
        public string? FatherLastname { get; set; }
        public string? MotherLastname { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        // Campos requeridos
        [Required(ErrorMessage = "El campo UserId es obligatorio")]
        public required Guid UserId { get; set; }
        [DefaultValue("2")]
        [Required(ErrorMessage = "El campo Role es obligatorio.")]
        public required int RoleId { get; set; }
        [DefaultValue(true)]
        [Required(ErrorMessage = "El campo Enabled es obligatorio.")]
        public required bool Enabled { get; set; }

    }
}
