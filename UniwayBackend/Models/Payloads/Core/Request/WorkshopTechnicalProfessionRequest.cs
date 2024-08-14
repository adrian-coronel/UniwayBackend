namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class WorkshopTechnicalProfessionRequest
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public int TechnicalProfessionId { get; set; }
        public bool Enabled { get; set; }
    }
}
