using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;

namespace SimbirSoft.Repositories.Implementations
{
    public class VisitedLocationRepository : Repository<VisitedLocation>, IVisitedLocationRepository
    {
        private readonly ApplicationDbContext _db;

        public VisitedLocationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

