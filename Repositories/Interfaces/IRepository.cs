using System.Linq.Expressions;

namespace SimbirSoft.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public T Get(int id);
        public void Add(T entity);
        public void Save();
        public void Update(T entity);
        public void Delete(T entity);
        public T FirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true);
    }
}
