using System.ComponentModel.DataAnnotations.Schema;
using UniwayBackend.Factories;

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
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        //public required bool WorkingStatus { get; set; }
        public required bool Enabled { get; set; }

    }
}
