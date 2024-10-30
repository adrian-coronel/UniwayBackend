using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace UniwayBackend.Models.Entities
{
    [Table("Technical")]
    public class Technical 
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string FatherLastname { get; set; }
        public required string MotherLastname { get; set; }
        public required string Dni { get; set; }
        public required DateTime BirthDate { get; set; }
        public Point? Location { get; set; }
        public bool WorkingStatus { get; set; }
        public string PhoneNumber { get; set; }
        public required bool Enabled { get; set; }



        // Inverse Relations
        public virtual List<Review> Reviews { get; set; } = new List<Review>();
        public virtual List<UserTechnical> UserTechnicals { get; set; } = new List<UserTechnical>();
    }
}
