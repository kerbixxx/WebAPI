using Microsoft.EntityFrameworkCore;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SimbirSoft.Repositories.Implementations
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        private readonly ApplicationDbContext _db;

        public AnimalRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Animal> GetAll(Expression<Func<Animal, bool>>? filter = null, Func<IQueryable<Animal>, IOrderedQueryable<Animal>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<Animal> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();
        }
    }
}
