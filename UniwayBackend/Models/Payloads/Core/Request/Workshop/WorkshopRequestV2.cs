using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request.Workshop
{
    public class WorkshopRequestV2
    {
        [Required(ErrorMessage = "El id del taller es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El id de la disponibilidad del técnico es requerido")]
        public int TechnicalId { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "La coordenada Latitud es requerida")]
        public double Lat { get; set; }
        [Required(ErrorMessage = "La coordenada Longitud es requerida")]
        public double Lng { get; set; }
        [Required(ErrorMessage = "El estado de trabajo es requerido")]
        public bool WorkingStatus { get; set; }
    }
}
