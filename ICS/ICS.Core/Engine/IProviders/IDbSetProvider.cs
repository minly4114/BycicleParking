using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Engine.IProviders
{
    public interface IDbSetProvider<T> where T : class
    {
        IDbSetProvider<T> Build(DbSet<T> dbSet, DbContext context);
        IQueryable<T> GetQueryable();

        Task<T> InsertWithReturnAsync(T obj);

        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task RemoveAsync(T obj);

        Task RemoveRangeAsync(List<T> objs);
    }
}
