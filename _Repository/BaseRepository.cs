using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.data;
using shop_giay_server.models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace shop_giay_server._Repository
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private DataContext _dataContext;

        public BaseRepository(DataContext context)
        {
            _dataContext= context;
        }

        public ValueTask<T> GetById(int id)
        {
            return _dataContext.Set<T>().FindAsync(id);
        }

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dataContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> Add(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Remove(int id)
        {
            var item = await _dataContext.Set<T>().FindAsync(id);

            if (item != null)
            {
                _dataContext.Set<T>().Remove(item);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<IEnumerable<T>> GetAll(IQueryCollection queries)
        {
            var result = _dataContext.Set<T>();

            // todo: generic filter
            var a = typeof(T).GetMembers();
            var b = typeof(T).GetProperties();
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => _dataContext.Set<T>().CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => _dataContext.Set<T>().CountAsync(predicate);

        Task<T> IAsyncRepository<T>.GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
