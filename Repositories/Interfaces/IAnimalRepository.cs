using SimbirSoft.Models;
using System.Linq.Expressions;

namespace SimbirSoft.Repositories.Interfaces
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        public List<Animal> GetAll(Expression<Func<Animal, bool>>? filter = null, Func<IQueryable<Animal>, IOrderedQueryable<Animal>>? orderBy = null, string? includeProperties = null, bool isTracking = true);
    }
}
