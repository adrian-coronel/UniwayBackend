namespace UniwayBackend.Models.Payloads.Core.Request.CategoryService
{
    public class CategoryServiceRequest
    {
        public short Id { get; set; }
        public string? Name { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
    }
}
