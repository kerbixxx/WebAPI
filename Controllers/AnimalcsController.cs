using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Data;
using SimbirSoft.Models;

namespace SimbirSoft.Controllers
{
    [Route("animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AnimalsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            if (id == 0) return BadRequest();
            var obj = await _db.Animals.FindAsync(id);
            if (obj == null) return NotFound();
            return obj;
        }
    }
}
