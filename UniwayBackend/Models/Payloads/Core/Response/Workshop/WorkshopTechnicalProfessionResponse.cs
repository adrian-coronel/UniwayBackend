namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class WorkshopTechnicalProfessionResponse
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public int TechnicalProfessionId { get; set; }
        public bool Enabled { get; set; }
    }
}
