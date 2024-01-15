using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Location
    {
        [Key]
        public long Id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
