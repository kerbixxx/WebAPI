using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SimbirSoft.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository _locationRepo;
        private readonly ILocationService _locationService;

        public LocationsController(ILocationRepository locationRepo, ILocationService locationService)
        {
            _locationRepo = locationRepo;
            _locationService = locationService;
        }

        [HttpGet("{pointId}")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Точка локации с таким pointId не найдена")]
        public ActionResult GetLocation(long pointId)
        {
            if (pointId <= 0) return BadRequest();
            var obj = _locationRepo.Get(pointId);
            if (obj == null) return NotFound();
            return new JsonResult((LocationResponse)obj);
        }

        [HttpPost]
        [SwaggerResponse(201, Type = typeof(LocationResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Точка локации с такими latitude и longitude уже существует")]
        public ActionResult AddLocation([FromBody]LocationRequest request)
        {
            if (!_locationService.IsValid(request)) return BadRequest();
            if (_locationService.IsConflict(request)) return Conflict();
            var obj = (Location)request;
            _locationRepo.Add(obj);
            _locationRepo.Save();
            return new JsonResult((LocationResponse)obj);
        }

        [HttpPut("{pointId}")]
        [SwaggerResponse(200, Type = typeof(LocationResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Точка локации с таким pointId не найдена")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Точка локации с такими latitude и longitude уже существует")]
        public ActionResult EditLocation(LocationRequest request, long pointId)
        {
            if (!_locationService.IsValid(request)) return BadRequest();
            var obj = _locationRepo.Get(pointId);
            if (obj == null) return NotFound();
            if (_locationService.IsConflict(request)) return Conflict();
            obj = (Location)request;
            _locationRepo.Update(obj);
            _locationRepo.Save();
            return new JsonResult((LocationResponse)obj);
        }

        [HttpDelete("{pointId}")]
        [SwaggerResponse(200, Type = typeof(LocationResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Точка локации с таким pointId не найдена")]
        public ActionResult DeleteLocation(long pointId)
        {
            if(pointId <= 0) return BadRequest();
            var obj = _locationRepo.Get(pointId);
            if (obj == null) return NotFound();
            _locationRepo.Delete(obj);
            _locationRepo.Save();
            return new JsonResult("Запрос успешно выполнен");
        }
    }
}
