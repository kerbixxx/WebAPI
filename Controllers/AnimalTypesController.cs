using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SimbirSoft.Controllers
{
    [Route("types")]
    [ApiController]
    public class AnimalTypesController : ControllerBase
    {
        private readonly IAnimalTypeRepository _animalTypeRepo;
        private readonly IAnimalTypeService _animalTypeService;

        public AnimalTypesController(IAnimalTypeRepository typeRepo, IAnimalTypeService animalTypeService)
        {
            _animalTypeRepo = typeRepo;
            _animalTypeService = animalTypeService;
        }

        [HttpGet("{typeId}")]
        [SwaggerResponse(200, Type = typeof(AnimalTypeResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Тип животного с таким typeId не найден")]
        public ActionResult GetType(long typeId)
        {
            if (typeId <= 0) return BadRequest();
            AnimalType obj = _animalTypeRepo.Get(typeId);
            if (obj == null) return NotFound();
            return new JsonResult((AnimalTypeResponse)obj);
        }

        [HttpPost]
        [SwaggerResponse(201, Type = typeof(AnimalTypeResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Тип животного с таким type уже существует")]
        public ActionResult AddAnimalType(AnimalTypeRequest request)
        {
            if (request.type == null || request.type == "") return BadRequest();
            if (_animalTypeService.IsConflict(request)) return Conflict();
            _animalTypeRepo.Add((AnimalType)request);
            _animalTypeRepo.Save();
            return new JsonResult((AnimalTypeResponse)(AnimalType)request);
        }

        [HttpPut("{typeId}")]
        [SwaggerResponse(201, Type = typeof(AnimalTypeResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Тип животного с таким typeId не найден")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Тип животного с таким type уже существует")]
        public ActionResult EditAnimalType(AnimalTypeRequest request, long typeId)
        {
            if (request.type == null || request.type == "") return BadRequest();    //400
            if (_animalTypeService.IsConflict(request)) return Conflict();          //409
            var obj = _animalTypeRepo.Get(typeId);
            if (obj == null) return NotFound();                                     //404
            else
            {
                obj = (AnimalType)request;
                obj.Id = typeId;
                _animalTypeRepo.Update(obj);
                _animalTypeRepo.Save();
                return new JsonResult((AnimalTypeResponse)obj);
            }
        }

        [HttpDelete("{typeId}")]
        public ActionResult DeleteAnimalType(long typeId)
        {
            if (typeId <= 0) return BadRequest();
            var obj = _animalTypeRepo.Get(typeId);
            if (obj == null) return NotFound();
            _animalTypeRepo.Delete(obj);
            _animalTypeRepo.Save();
            return new JsonResult("Запрос успешно выполнен");
        }
    }
}
