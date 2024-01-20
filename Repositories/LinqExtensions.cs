using SimbirSoft.Models;
using System.Linq.Expressions;

namespace SimbirSoft.Repositories
{
    public static class AccountListExtensions
    {
        public static List<Account> WhereIf(
            this List<Account> source,
            bool condition,
            Func<Account, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate).ToList();
            }
            return source;
        }
    }
}
