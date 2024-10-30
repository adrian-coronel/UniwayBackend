using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniwayBackend.Factories;

namespace UniwayBackend.Models.Entities
{
    [Table("Client")]
    public class Client 
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string FatherLastname { get; set; }
        public required string MotherLastname { get; set; }
        public required string Dni { get; set; }
        public required DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public required bool Enabled { get; set; }

        [NotMapped]
        public User User { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
