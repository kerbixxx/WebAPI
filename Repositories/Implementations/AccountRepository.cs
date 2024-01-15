using Microsoft.EntityFrameworkCore;
using SimbirSoft.Data;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SimbirSoft.Repositories.Implementations
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Account FirstOrDefault(Func<Account, bool> func)
        {
            return _db.Accounts.FirstOrDefault(func);
        }

        public List<Account> GetAll(Expression<Func<Account, bool>>? filter = null, Func<IQueryable<Account>, IOrderedQueryable<Account>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<Account> query = dbSet;
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
