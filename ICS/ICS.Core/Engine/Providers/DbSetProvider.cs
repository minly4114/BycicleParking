using ICS.Core.Engine.IProviders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Engine.Providers
{
    public class DbSetProvider<T> : IDbSetProvider<T> where T : class
    {
        private DbContext _context;
        private DbSet<T> _dbset;

        public IDbSetProvider<T> Build(DbSet<T> dbSet, DbContext context)
        {
            _context = context;
            _dbset = dbSet;
            return this;
        }
        public IQueryable<T> GetQueryable()
        {
            return _dbset.AsQueryable();
        }
        public async Task InsertAsync(T obj)
        {
            await _dbset.AddAsync(obj);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T obj)
        {
            _dbset.Update(obj);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAsync(T obj)
        {
            _dbset.Remove(obj);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveRangeAsync(List<T> objs)
        {
            _dbset.RemoveRange(objs);
            await _context.SaveChangesAsync();
        }

        public async Task<T> InsertWithReturnAsync(T obj)
        {
            var objDb = _dbset.AddAsync(obj).Result.Entity;
            await _context.SaveChangesAsync();
            return objDb;
        }
    }
}
