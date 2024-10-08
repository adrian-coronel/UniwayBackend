using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request.Technical
{
    public class TechnicalRequestV1
    {
        [Required(ErrorMessage ="El id del técnico es requerido")]
        public int TechnicalId { get; set; }
        [Required(ErrorMessage = "El estado de trabajo es requerido")]
        public bool WorkingStatus { get; set; }
        public int? Distance { get; set; } = 5000;
        [Required(ErrorMessage = "La coordenada Latitud es requerida")]
        public double Lat { get; set; }
        [Required(ErrorMessage = "La coordenada Longitud es requerida")]
        public double Lng { get; set; }
    }
}
