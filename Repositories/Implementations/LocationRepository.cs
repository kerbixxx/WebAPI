using Microsoft.EntityFrameworkCore;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;
namespace SimbirSoft.Repositories.Implementations
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
