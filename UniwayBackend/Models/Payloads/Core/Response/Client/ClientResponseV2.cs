namespace UniwayBackend.Models.Payloads.Core.Response.Client
{
    public class ClientResponseV2
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string FatherLastname { get; set; }
        public required string MotherLastname { get; set; }
        public required string Dni { get; set; }
        public required DateTime BirthDate { get; set; }
        public required bool Enabled { get; set; }
    }
}
