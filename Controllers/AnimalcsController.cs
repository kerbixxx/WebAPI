using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services;
using SimbirSoft.Services.Implementations;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;

namespace SimbirSoft.Controllers
{
    [Route("animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepo;

        public AnimalsController(IAnimalRepository animalRepo)
        {
            _animalRepo = animalRepo;
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(AnimalResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Проблема валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Животное с animalId не найдено")]
        public ActionResult GetAnimal(long id)
        {
            if (id <= 0) return BadRequest();
            var obj = _animalRepo.Get(id);
            if (obj == null) return NotFound();
            return new JsonResult((AnimalResponse)obj);
        }

        [Authorize]
        [HttpGet]
        [Route("search")]
        [SwaggerResponse(200, Type = typeof(List<AnimalResponse>), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Проблема валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Животное с animalId не найдено")]
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
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<AnimalResponse>), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Проблема валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
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
    }
}
