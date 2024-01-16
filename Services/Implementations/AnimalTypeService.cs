using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using System;

namespace SimbirSoft.Services.Implementations
{
    public class AnimalTypeService : IAnimalTypeService
    {
        private readonly IAnimalTypeRepository _animalTypeRepo;

        public AnimalTypeService(IAnimalTypeRepository animalTypeRepo)
        {
            _animalTypeRepo = animalTypeRepo;
        }

        public bool IsConflict(AnimalTypeRequest request)
        {
            return _animalTypeRepo.FirstOrDefault(t => t.type == request.type) != null;
        }
    }
}
