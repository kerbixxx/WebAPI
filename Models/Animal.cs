using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Animal
    {
        [Key]
        public long Id { get; set; }

        public long[] animalTypesId { get; set; }
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
        public long[] visitedLocationsId { get; set; }
        public IEnumerable<VisitedLocation>? VisitedLocations { get; set; }
        public DateTime deathDateTime { get; set; }

        public static explicit operator AnimalResponse(Animal animal)
        {
            return new AnimalResponse
            {
                Id = animal.Id,
                AnimalTypes = animal.animalTypesId.ToArray(),
                Weight = animal.weight,
                Length = animal.length,
                Height = animal.height,
                Gender = animal.gender,
                LifeStatus = animal.lifestatus,
                ChipperId = animal.chipperId,
                ChippingDateTime = animal.chippingDateTime,
                ChippingLocationId = animal.chippingLocationId,
                VisitedLocations = animal.visitedLocationsId,
                DeathDateTime = animal.deathDateTime
            };
        }
    }

    public class AnimalResponse
    {
        public long Id { get; set; }
        public long[] AnimalTypes { get; set; }
        public float Weight {  get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Gender {  get; set; }
        public string LifeStatus {  get; set; }
        public DateTime ChippingDateTime { get; set; }
        public int ChipperId { get; set;}
        public long ChippingLocationId { get; set; }
        public long[] VisitedLocations {  get; set; }
        public DateTime DeathDateTime {  get; set; }


    }
}
