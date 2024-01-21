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

        public int AccountId { get; set; }
        public Account? Account{ get; set; }
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
                ChipperId = animal.AccountId,
                ChippingDateTime = animal.chippingDateTime,
                ChippingLocationId = animal.chippingLocationId,
                VisitedLocations = animal.visitedLocationsId,
                DeathDateTime = animal.deathDateTime
            };
        }
    }

    public class AnimalRequest
    {
        public long[] AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; }
        public int AccountId { get; set; }
        public long ChippingLocationId {  get; set; }

        public static explicit operator Animal(AnimalRequest request)
        {
            return new Animal
            {
                animalTypesId = request.AnimalTypes,
                weight = request.Weight,
                length = request.Length,
                height = request.Height,
                gender = request.Gender,
                AccountId = request.AccountId,
                chippingLocationId = request.ChippingLocationId,
                lifestatus = "ALIVE"
            };
        }
    }

    public class AnimalResponse
    {
        public AnimalResponse(long id, long[] animalTypesId, float weight, float length, float height, string gender, string lifestatus, DateTime chippingDateTime, int chipperId, long chippingLocationId, long[] visitedLocationsId, DateTime deathDateTime)
        {
            Id = id;
            AnimalTypesId = animalTypesId;
            Weight = weight;
            Length = length;
            Height = height;
            Gender = gender;
            LifeStatus = lifestatus;
            ChippingDateTime = chippingDateTime;
            ChipperId = chipperId;
            ChippingLocationId = chippingLocationId;
            VisitedLocationsId = visitedLocationsId;
            DeathDateTime = deathDateTime;
        }

        public AnimalResponse() { }

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
        public long[] AnimalTypesId { get; }
        public long[] VisitedLocationsId { get; }

    }
    public class AnimalTypeRequestChange()
    {
        public long oldTypeId { get; set; }
        public long newTypeId { get; set; }
    }
}
