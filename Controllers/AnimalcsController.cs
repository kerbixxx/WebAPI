using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services;
using SimbirSoft.Services.Implementations;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;

namespace SimbirSoft.Controllers
{
    [Route("animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IAnimalService _animalService;
        private readonly IAnimalTypeRepository _animalTypeRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly IVisitedLocationRepository _visitedLocationRepo;

        public AnimalsController(IAnimalRepository animalRepo, IAnimalService animalService, IAnimalTypeRepository animalTypeRepo, IAccountRepository accountRepo, ILocationRepository locationRepo, IVisitedLocationRepository visitedLocationRepo)
        {
            _animalRepo = animalRepo;
            _animalService = animalService;
            _animalTypeRepo = animalTypeRepo;
            _accountRepo = accountRepo;
            _locationRepo = locationRepo;
            _visitedLocationRepo = visitedLocationRepo;
        }

        [Authorize]
        [HttpGet("{animalId}")]
        public ActionResult GetAnimal(long animalId)
        {
            if (animalId <= 0) return BadRequest();
            var obj = _animalRepo.Get(animalId);
            if (obj == null) return NotFound();
            return new JsonResult((AnimalResponse)obj);
        }

        [Authorize]
        [HttpGet]
        [Route("search")]
        public ActionResult SearchAnimals([FromQuery]DateTime? startDateTime, [FromQuery]DateTime? endDateTime, [FromQuery]int? chipperId,
            [FromQuery]long? chippingLocationId, [FromQuery]string? lifeStatus, [FromQuery]string? gender, [FromQuery]int? from, [FromQuery]int? size)
        {
            if (from < 0 || size <= 0) return BadRequest();
            if (size == null) size = 10;
            if (from == null) from = 0;
            var obj = _animalRepo.GetAll();

            if (startDateTime.HasValue && endDateTime.HasValue)
                obj = obj.Where(a=>a.chippingDateTime >= startDateTime.Value && a.chippingDateTime <= endDateTime.Value).ToList();

            if (chipperId != null) obj = obj.Where(a => a.chipperId == chipperId).ToList();
            if (!string.IsNullOrWhiteSpace(lifeStatus) || lifeStatus == "ALIVE" || lifeStatus == "DEAD")
                obj = obj.Where(a => a.lifestatus == lifeStatus).ToList();
            if (chippingLocationId != null) obj = obj.Where(a => a.chippingLocationId == chippingLocationId).ToList();
            if (!string.IsNullOrWhiteSpace(gender) || gender == "MALE" || gender == "FEMALE" || gender == "OTHER")
                obj = obj.Where(a => a.gender == gender).ToList();
            
            obj = obj.Skip((int)from).Take((int)size).ToList();
            var animalResponses = obj.Select(a => new AnimalResponse(a.Id, a.animalTypesId, a.weight, a.length, a.height, a.gender, a.lifestatus, a.chippingDateTime,
                a.chipperId, a.chippingLocationId, a.visitedLocationsId, a.deathDateTime)).ToList();
            return new JsonResult(animalResponses);
        }

        [Authorize]
        [HttpGet("{animalId}/locations")]
        public ActionResult GetLocationsForAnimal(long animalId, [FromQuery] DateTime? startDateTime, [FromQuery] DateTime? endDateTime,
            [FromQuery] int? from, [FromQuery] int? size)
        {
            if (animalId <= 0 || size <= 0 || from < 0) return BadRequest();
            if (size == null) size = 10;
            if (from == null) from = 0;
            var obj = _animalRepo.FirstOrDefault(a => a.Id == animalId, includeProperties: "VisitedLocations");
            if (obj == null || obj.VisitedLocations == null) return NotFound();

            // Применяем фильтрацию по датам если заданы
            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                obj.VisitedLocations = obj.VisitedLocations
                    .Where(v => v.dateTimeOfVisitLocationPoint >= startDateTime.Value && v.dateTimeOfVisitLocationPoint <= endDateTime.Value)
                    .ToList();
            }

            // Применяем пагинацию
            obj.VisitedLocations = obj.VisitedLocations
                .Skip(from.GetValueOrDefault())
                .Take(size.GetValueOrDefault())
                .ToList();

            List<VisitedLocationResponse> locations = obj.VisitedLocations
                .Select(location => new VisitedLocationResponse
                {
                    Id = location.Id,
                    dateTimeOfVisitLocationPoint = location.dateTimeOfVisitLocationPoint,
                    locationPointId = location.locationPointId
                })
                .ToList();

            return new JsonResult(locations);
        }

        [HttpPost]
        public ActionResult AddAnimal(AnimalRequest request)
        {
            if (!_animalService.IsValid(request)) return BadRequest();
            foreach(var obj in request.AnimalTypes)
            {
                if (_animalTypeRepo.Get(obj) == null) return NotFound();
            }
            if (_accountRepo.Get(request.chipperId) == null) return NotFound();
            if (_locationRepo.Get(request.ChippingLocationId) == null) return NotFound();
            _animalRepo.Add((Animal)request);
            _animalRepo.Save();
            return new JsonResult((AnimalResponse)(Animal)request);
        }

        [HttpPut("{animalId}")]
        public ActionResult UpdateAnimal(AnimalRequest request, long animalId)
        {
            if (!_animalService.IsValid(request)) return BadRequest();
            foreach (var obj in request.AnimalTypes)
            {
                if (_animalTypeRepo.Get(obj) == null) return NotFound();
            }
            if (_accountRepo.Get(request.chipperId) == null) return NotFound();
            if (_locationRepo.Get(request.ChippingLocationId) == null) return NotFound();
            var animal = _animalRepo.Get(animalId);
            animal = (Animal)request;
            _animalRepo.Update(animal);
            _animalRepo.Save();
            return new JsonResult((AnimalResponse)animal);
        }

        [HttpDelete("{animalId}")]
        public ActionResult DeleteAnimal(long animalId)
        {
            if (animalId <= 0) return BadRequest();
            //Животное покинуло локацию чипирования, при этом есть другие посещенные точки. ????
            var obj = _animalRepo.Get(animalId);
            if (obj == null) return NotFound();
            _animalRepo.Delete(obj);
            _animalRepo.Save();
            return new JsonResult("Запрос успешно выполнен");
        }

        [HttpPost("{animalId}/types/{typeId}")]
        public ActionResult AddTypeToAnimal(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0) return BadRequest();
            var animal = _animalRepo.Get(animalId);
            if (animal == null) return NotFound();
            var type = _animalTypeRepo.Get(animalId);
            if (type == null) return NotFound();
            animal.animalTypesId.Append(typeId);
            _animalRepo.Update(animal);
            _animalRepo.Save();
            return new JsonResult((AnimalResponse)animal);
        }

        [HttpPut("{animalId}/types")]
        public ActionResult UpdateTypeAnimal(long animalId, AnimalTypeRequestChange request)
        {
            if (animalId <= 0 || request.oldTypeId <= 0 || request.newTypeId <= 0) return BadRequest();
            var animal = _animalRepo.FirstOrDefault(a=>a.Id==animalId,includeProperties: "animalTypes");
            if (animal == null) return NotFound();
            var oldType = _animalTypeRepo.Get(request.oldTypeId);
            if (oldType == null) return NotFound();
            var newType = _animalTypeRepo.Get(request.newTypeId);
            if (newType == null) return NotFound();
            if (animal.animalTypesId.Contains(newType.Id)) return Conflict();
            int index = Array.IndexOf(animal.animalTypesId, oldType.Id);
            animal.animalTypesId[index] = request.newTypeId;
            _animalRepo.Update(animal);
            _animalRepo.Save();
            return new JsonResult((AnimalResponse)animal);
        }

        [HttpDelete("{animalId}/types/{typeId}")]
        public ActionResult DeleteTypeAnimal(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0) return BadRequest();
            var animal = _animalRepo.Get(animalId);
            if (animal == null) return NotFound();
            var type = _animalTypeRepo.Get(animalId);
            if (type == null) return NotFound();
            if (!animal.animalTypesId.Contains(type.Id)) return NotFound();
            int index = Array.IndexOf(animal.animalTypesId, typeId);
            var tmp = new long[animal.animalTypesId.Length - 1];
            for(int i = 0; i < animal.animalTypesId.Length; i++)
            {
                if(i<index) tmp[i] = animal.animalTypesId[i];
                else if(i>index) tmp[i = 1] = animal.animalTypesId[i];
            }
            animal.animalTypesId = tmp;
            _animalRepo.Update(animal);
            _animalRepo.Save();
            return new JsonResult((AnimalResponse)animal);
        }

        [HttpPost("{animalId}/locations/{pointId}")]
        public ActionResult AddVisitedLocationToAnimal(long animalId, long pointId)
        {
            if(animalId<=0 || pointId<=0) return BadRequest();
            var animal = _animalRepo.Get(animalId);
            if (animal == null) return NotFound();
            if (animal.lifestatus == "DEAD") return BadRequest();
            var location = _locationRepo.Get(pointId);
            if (location == null) return NotFound();
            VisitedLocation visitedLocation = new();
            visitedLocation.locationPointId = pointId;
            DateTime visitLocation = DateTime.Now;
            _visitedLocationRepo.Add(visitedLocation);
            _visitedLocationRepo.Save();
            animal.VisitedLocations.Append(visitedLocation);
            return new JsonResult(visitedLocation);
        }

        [HttpPut("{animalId}/locations")]
        public ActionResult UpdateVisitedLocationAnimal(long animalId, VisitedLocationRequest request)
        {
            if(animalId<=0 || request==null || request.visitedLocationPointId<=0 || request.LocationPointId <= 0) return BadRequest();
            var animal = _animalRepo.Get(animalId);
            if (animal == null) return NotFound();
            var location = _locationRepo.Get(request.LocationPointId);
            if (location == null) return NotFound();
            var visitedLocation = _visitedLocationRepo.Get(request.visitedLocationPointId);
            return new JsonResult("яустал)");
        }

        [HttpDelete("{animalId}/locations/{visitedPointId}")]
        public ActionResult DeleteVisitedLocationAnimal(long animalId, long visitedPointId)
        {
            if(animalId<=0 || visitedPointId<=0) return NotFound();
            var animal = _animalRepo.Get(animalId);
            if (animal == null) return NotFound();
            var visitedPoint = _visitedLocationRepo.Get(visitedPointId);
            if (visitedPoint == null) return NotFound();
            if (!animal.visitedLocationsId.Contains(visitedPoint.Id)) return NotFound();
            _visitedLocationRepo.Delete(visitedPoint);
            _visitedLocationRepo.Save();
            return new JsonResult("Запрос выполнен");
        }
    }
}
