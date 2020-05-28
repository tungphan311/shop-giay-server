using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.data;
using shop_giay_server.models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Primitives;

namespace shop_giay_server._Repository
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private DataContext _dataContext;

        private int defaultPageSize = 20;
        private int defaultPageNumber = 1;

        public BaseRepository(DataContext context)
        {
            _dataContext= context;
        }

        public async Task<T> GetById(int id)
        {
            return await FirstOrDefault(o => o.Id == id);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> Add(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> Add(IEnumerable<T> entities)
        {
            await _dataContext.Set<T>().AddRangeAsync(entities);
            await _dataContext.SaveChangesAsync();
            return entities;
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

        public async Task<IEnumerable<T>> GetAll(Dictionary<string, StringValues> queries)
        {
            var query = _dataContext.Set<T>().AsQueryable();
            var dict = queries.ToDictionary(
                p => p.Key.ToLower(),
                p => p.Value.ToString());

            var pageSize = -1;
            var pageNumber = -1;


            // Paging
            this.GetPageInfo(dict, ref pageNumber, ref pageSize);
            if (pageSize != -1 || pageNumber != -1)
            {
                if (pageSize == -1) pageSize = defaultPageSize;
                if (pageNumber == -1) pageNumber = defaultPageNumber;

                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            

            // Filter by queries
            foreach (var p in _dataContext.Model.FindEntityType(typeof(T)).GetProperties())
            {
                if (!dict.ContainsKey(p.Name.ToLower())) continue;

                var rawVl = dict[p.Name.ToLower()];
                var type = p.PropertyInfo.PropertyType;
                try
                {
                    var vl = Convert.ChangeType(rawVl, type);
                    query = query.Where(String.Format($"{p.Name} = {vl}"));
                }
                catch
                {
                    Console.WriteLine($"[INFO] Cannot convert {rawVl} to type {type} while applying query: {p.Name}");
                }
            }


            // Included all navigations properties
            foreach (var p in _dataContext.Model.FindEntityType(typeof(T)).GetNavigations())
                query = query.Include(p.Name);


            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<int> CountAll() => await _dataContext.Set<T>().CountAsync();

        public async Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => await _dataContext.Set<T>().CountAsync(predicate);

        public async Task<bool> ExistWhere(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().AnyAsync(predicate);
        }

        //
        //  Helper class
        //
        private void GetPageInfo(Dictionary<string, string> queries, ref int pageNumber, ref int pageSize)
        {
            string pNumb = null;
            string pSize = null;

            if (queries.TryGetValue("pagesize", out pSize)) {
                var value = -1;
                Int32.TryParse(pSize, out value);
                if (value > 0) pageSize = value;
            }
            if (queries.TryGetValue("page", out pNumb)) {
                var value = -1;
                Int32.TryParse(pNumb, out value);
                if (value > 0) pageNumber = value;
            }

        }
        
    }
}
