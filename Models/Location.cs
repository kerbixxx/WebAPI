using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Location
    {
        [Key]
        public long Id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public static explicit operator LocationResponse(Location location)
        {
            return new LocationResponse
            {
                Id = location.Id,
                latitude = location.latitude,
                longitude = location.longitude
            };
        }
    }

    public class LocationRequest
    {
        public double latitude;
        public double longitude;

        public static explicit operator Location(LocationRequest location)
        {
            return new Location
            {
                latitude = location.latitude,
                longitude = location.longitude
            };
        }
    }

    public class LocationResponse
    {
        public long Id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
