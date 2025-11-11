
using SQLite;

namespace MHikeHybrid.Models
{
    [Table("hikes")] 
    public class Hike
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 

        [Column("name"), NotNull]
        public string Name { get; set; }

        [Column("location"), NotNull]
        public string Location { get; set; }

        [Column("hike_date"), NotNull]
        public string HikeDate { get; set; } 

        [Column("parking_available"), NotNull]
        public bool ParkingAvailable { get; set; } 

        [Column("length"), NotNull]
        public string Length { get; set; }

        [Column("difficulty"), NotNull]
        public string Difficulty { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("transport")]
        public string Transport { get; set; } 

        [Column("est_time")]
        public string EstTime { get; set; }   
    }
}