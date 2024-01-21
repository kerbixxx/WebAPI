using SimbirSoft.Models;
using SimbirSoft.Services.Interfaces;

namespace SimbirSoft.Services.Implementations
{
    public class AnimalService : IAnimalService
    {
        public bool IsValid(AnimalRequest request)
        {
            for(int i = 0; i < request.AnimalTypes.Length; i++)
            {
                if (request.AnimalTypes[i] < 0) return false;
            }
            return !(request.AnimalTypes == null) && !(request.AnimalTypes.Length <= 0) && !(request.Weight <= 0) && !(request.Length<=0)&&
                !(request.Height<=0)&&((request.Gender=="MALE") || (request.Gender == "FEMALE") || (request.Gender == "OTHER"))&&
                !(request.chipperId<=0)&&!(request.ChippingLocationId<=0);
        }
    }
}
