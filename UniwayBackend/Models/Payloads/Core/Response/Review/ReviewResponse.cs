using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class ReviewResponse
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public int TechnicalId { get; set; }

        public int ClientId { get; set; }

        public short StarNumber { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
