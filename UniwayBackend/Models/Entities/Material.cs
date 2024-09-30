using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Material")]
    public class Material
    {
        public int Id { get; set; }
        public int TechnicalResponseId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public short Stock { get; set; }

        [ForeignKey("TechnicalResponseId")]
        public virtual TechnicalResponse TechnicalResponse { get; set; }
    }
}
