using SimbirSoft.Models;
using System.Linq.Expressions;

namespace SimbirSoft.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account FirstOrDefault(Func<Account, bool> func);
        public List<Account> GetAll(Expression<Func<Account, bool>>? filter = null, Func<IQueryable<Account>, IOrderedQueryable<Account>>? orderBy = null, string? includeProperties = null, bool isTracking = true);
    }
}
