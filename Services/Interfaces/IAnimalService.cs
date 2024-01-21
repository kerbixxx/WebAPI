using SimbirSoft.Models;

namespace SimbirSoft.Services.Interfaces
{
    public interface IAnimalService
    {
        public bool IsValid(AnimalRequest request);
    }
}
