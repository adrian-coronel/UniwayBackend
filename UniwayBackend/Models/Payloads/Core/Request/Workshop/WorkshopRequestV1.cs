using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request.Workshop
{
    public class WorkshopRequestV1
    {
        [Required(ErrorMessage = "El id del taller es requerido")]
        public int WorkshopId { get; set; }
        [Required(ErrorMessage = "El estado de trabajo es requerido")]
        public bool WorkingStatus { get; set; }
        public string Description { get; set; }
        public int? Distance { get; set; } = 5000;
        [Required(ErrorMessage = "La coordenada Latitud es requerida")]
        public double Lat { get; set; }
        [Required(ErrorMessage = "La coordenada Longitud es requerida")]
        public double Lng { get; set; }
    }
}
