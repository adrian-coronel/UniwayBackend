namespace UniwayBackend.Models.Dtos
{
    public class WorkshopTechnicalProfessionDto
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public int TechnicalProfessionId { get; set; }
        public bool Enabled { get; set; }
    }
}
