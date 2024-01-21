using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SimbirSoft.Controllers
{
    [Route("locations")]
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

        [Authorize]
        [HttpGet("{pointId}")]
        public ActionResult GetLocation(long pointId)
        {
            if (pointId <= 0) return BadRequest();
            var obj = _locationRepo.Get(pointId);
            if (obj == null) return NotFound();
            return new JsonResult((LocationResponse)obj);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLocation([FromBody]LocationRequest request)
        {
            if (!_locationService.IsValid(request)) return BadRequest();
            if (_locationService.IsConflict(request)) return Conflict();
            var obj = (Location)request;
            _locationRepo.Add(obj);
            _locationRepo.Save();
            return StatusCode(StatusCodes.Status201Created,(LocationResponse)obj);
        }

        [Authorize]
        [HttpPut("{pointId}")]
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

        [Authorize]
        [HttpDelete("{pointId}")]
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
