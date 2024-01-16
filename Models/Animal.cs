using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Animal
    {
        [Key]
        public long Id { get; set; }
        public IEnumerable<AnimalType> animalTypes { get; set; }
        public float weight { get; set; }
        public float length { get; set; }
        public float height { get; set; }
        public string gender { get; set; }
        public string lifestatus { get; set; }
        public DateTime chippingDateTime { get; set; }
        public int chipperId { get; set; }
        public Chipper? chipper { get; set; }
        public long chippingLocationId { get; set; }
        public long[] visiterLocations { get; set; }
        public DateTime deathDateTime { get; set; }
    }
}
