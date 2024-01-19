using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
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

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(AnimalResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Проблема валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Животное с animalId не найдено")]
        public ActionResult GetAnimal(int id)
        {
            if (id <= 0) return BadRequest();
            var obj = _animalRepo.Get(id);
            if (obj == null) return NotFound();
            return Ok(new JsonResult((AnimalResponse)obj));
        }

        [HttpGet]
        public ActionResult SearchAnimals([FromQuery] DateTime startDateTime, [FromQuery] DateTime endDateTime, [FromQuery] int chipperId,
            [FromQuery] long chippingLocationId, [FromQuery] string lifeStatus, [FromQuery] string gender, [FromQuery] int from, [FromQuery] int size)
        {
            if (from < 0 || size <= 0 || !AreDatesISO0861(startDateTime,endDateTime) || chipperId <= 0 || lifeStatus != "ALIVE" || lifeStatus != "DEAD"
                || gender != "MALE" || gender != "FEMALE" || gender != "OTHER")
            {
                return BadRequest();
            }
            var animals = _animalRepo.GetAll()
                .Where(a => a.chippingDateTime > startDateTime || a.chippingDateTime < endDateTime)
                .Where(a => a.chipperId == chipperId)
                .Where(a => a.chippingLocationId == chippingLocationId)
                .Where(a => a.lifestatus == lifeStatus)
                .Where(a => a.gender == gender).ToList();
            List<AnimalResponse> objResponses = new();
            if (animals.Count()<size+from) size = animals.Count();
            for (int i = from; i < size; i++)
            {
                objResponses.Add((AnimalResponse)animals[i]);
            }
            return Ok(new JsonResult(objResponses));
        }

        [HttpGet("{animalId}/locations")]
        public ActionResult GetLocationsForAnimal(int animalId, [FromQuery]DateTime startDateTime, [FromQuery]DateTime endDateTime,
            [FromQuery]int from, [FromQuery]int size)
        {
            if (animalId <= 0 || from < 0 || size <= 0 || !AreDatesISO0861(startDateTime, endDateTime)) return BadRequest();
            var obj = _animalRepo.FirstOrDefault(a=>a.Id == animalId,includeProperties:"VisitedLocations");
            if(obj == null) return NotFound();
            List<VisitedLocationResponse> locations = new();
            if (obj.VisitedLocations != null)
                foreach (var location in obj.VisitedLocations)
                {
                    locations.Add(new VisitedLocationResponse { Id = location.Id, dateTimeOfVisitLocationPoint = location.dateTimeOfVisitLocationPoint, locationPointId = location.locationPointId});
                }
            else return NotFound();
            return Ok(new JsonResult(locations));
        }

        public bool AreDatesISO0861(DateTime startDateTime, DateTime endDateTime)
        {
            string format = "o"; // Round-trip ("O", "o") format specifier
            CultureInfo culture = CultureInfo.InvariantCulture; // Use the invariant culture

            string dateString1 = startDateTime.ToString(format, culture);
            string dateString2 = endDateTime.ToString(format, culture);

            DateTime parsedDateTime1;
            DateTime parsedDateTime2;

            bool isValidParam1 = DateTime.TryParseExact(dateString1, format, culture, DateTimeStyles.RoundtripKind, out parsedDateTime1);
            bool isValidParam2 = DateTime.TryParseExact(dateString2, format, culture, DateTimeStyles.RoundtripKind, out parsedDateTime2);

            return isValidParam1 && isValidParam2;
        }
    }
}
