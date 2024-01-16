using SimbirSoft.Models;

namespace SimbirSoft.Services.Interfaces
{
    public interface IAnimalTypeService
    {
        public bool IsConflict(AnimalTypeRequest request);
    }
}
