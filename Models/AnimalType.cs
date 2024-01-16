using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class AnimalType
    {
        [Key]
        public long Id { get; set; }
        public string type { get; set; }

        public static explicit operator AnimalTypeResponse(AnimalType obj)
        {
            return new AnimalTypeResponse
            {
                Id = obj.Id,
                type = obj.type,
            };
        }
    }

    public class AnimalTypeRequest
    {
        public string type { get; set; }

        public static explicit operator AnimalType(AnimalTypeRequest obj)
        {
            return new AnimalType
            {
                type = obj.type
            };
        }
    }

    public class AnimalTypeResponse
    {
        public long Id { get; set; }
        public string type { get; set; }
    }
}
