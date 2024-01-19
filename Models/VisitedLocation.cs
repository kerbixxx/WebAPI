using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class VisitedLocation
    {
        [Key]
        public long Id { get; set; }
        public DateTime dateTimeOfVisitLocationPoint { get; set; }

        public long locationPointId { get; set; }
        public Location? locationPoint { get; set; }
    }

    public class VisitedLocationResponse
    {
        public long Id { get; set; }
        public DateTime? dateTimeOfVisitLocationPoint { get; set; }
        public long locationPointId { get; set;}
    }
}
