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
        private string pageSizeKey = "page-size";
        private string pageNumberKey = "page";

        public BaseRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<T> GetById(int id, bool loadAllNavProperties = true)
        {
            return await FirstOrDefault(o => o.Id == id, loadAllNavProperties);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, bool loadAllNavProperties = true)
        {
            var query = _dataContext.Set<T>().AsQueryable(); //.FirstOrDefaultAsync(predicate);

            if (loadAllNavProperties)
            {
                foreach (var p in _dataContext.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(p.Name);
            }

            return await query.FirstOrDefaultAsync(predicate);
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
            var updateEntity = _dataContext.Entry(await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id));
            if (updateEntity == null) return null;

            updateEntity.CurrentValues.SetValues(entity);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Set<T>().FirstOrDefaultAsync(c => c.Id == entity.Id);
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

        public async Task<(IEnumerable<T> result, int totalRecords)> GetAllWithQuery(IQueryCollection query, bool loadAllNavProperties = true)
        {
            var dict = query.ToDictionary(
                p => p.Key.ToLower(),
                p => p.Value);

            return await GetAll(dict, loadAllNavProperties);
        }

        public async Task<(IEnumerable<T> result, int totalRecords)> GetAll(Dictionary<string, StringValues> dictQuery, bool loadAllNavProperties = true)
        {
            var query = _dataContext.Set<T>().AsQueryable();
            var pageSize = -1;
            var pageNumber = -1;


            // Filter by queries
            AddCompareQueries(ref query, dictQuery, "=");
            AddCompareQueries(ref query, dictQuery, "<=");
            AddCompareQueries(ref query, dictQuery, ">=");
            AddSearchQueries(ref query, dictQuery);

            // Get total records without paging
            var totalRecords = await query.CountAsync();


            // Add paging
            GetPageInfo(dictQuery, ref pageNumber, ref pageSize);
            if (pageSize != -1 || pageNumber != -1)
            {
                if (pageSize == -1) pageSize = defaultPageSize;
                if (pageNumber == -1) pageNumber = defaultPageNumber;

                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }


            // Included all navigations properties
            if (loadAllNavProperties)
            {
                foreach (var p in _dataContext.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(p.Name);
            }

            // Ad-hoc: size query for shoes
            if (typeof(T) == typeof(Shoes) && dictQuery.Keys.Contains("sizeid"))
            {
                var value = dictQuery["sizeid"][0];
                var _value = int.Parse(value);
                IQueryable<Shoes> _query = (IQueryable<Shoes>)query;
                query = (IQueryable<T>)_query.Where(c => c.Stocks.Where(c => c.SizeId == _value).Count() >= 1);
            }

            var listResult = await query.ToListAsync();
            return (result: listResult, totalRecords: totalRecords);
        }


        private void AddCompareQueries(ref IQueryable<T> query, Dictionary<string, StringValues> dictQuery, string operatorString)
        {
            switch (operatorString)
            {
                case ">=":
                case ">":
                    dictQuery = dictQuery.Where(kv => kv.Key.StartsWith("min"))
                            .ToDictionary(kv => kv.Key.Substring(3), kv => kv.Value);
                    break;
                case "<":
                case "<=":
                    dictQuery = dictQuery.Where(kv => kv.Key.StartsWith("max"))
                            .ToDictionary(kv => kv.Key.Substring(3), kv => kv.Value);
                    break;
                default:
                    break;
            }

            foreach (var p in _dataContext.Model.FindEntityType(typeof(T)).GetProperties())
            {
                if (!dictQuery.ContainsKey(p.Name.ToLower())) continue;

                var rawVl = dictQuery[p.Name.ToLower()][0];
                var type = p.PropertyInfo.PropertyType;

                try
                {
                    dynamic vl = null;
                    if (type == typeof(bool))
                    {
                        vl = (rawVl == "1") || (rawVl == "true");
                    }
                    else
                    {
                        vl = Convert.ChangeType(rawVl, type);
                    }

                    // Handle equal comparing with float value.
                    if (type == typeof(float) && operatorString == "=")
                    {
                        var odd = 0.001;
                        var substractResult = String.Format($"{p.Name} - @0");
                        var queryString = String.Format($"({substractResult} >= 0 && {substractResult} < {odd}) || ({substractResult} < 0 && {substractResult} > {odd * -1})");
                        query = query.Where(queryString, (float)vl);
                    }
                    else
                    {
                        query = query.Where(String.Format($"{p.Name} {operatorString} {vl}"));
                    }
                }
                catch
                {
                    Console.WriteLine($"[INFO] Cannot convert {rawVl} to type {type} while applying query: {p.Name}");
                }
            }
        }

        private void AddSearchQueries(ref IQueryable<T> query, Dictionary<string, StringValues> dictQuery)
        {
            var searchKey = "search";
            var isShoesType = typeof(T) == typeof(Shoes);
            var hasKeyword = dictQuery.ContainsKey(searchKey);
            if (!isShoesType || !hasKeyword)
            {
                return;
            }

            var value = dictQuery[searchKey][0];
            query = query.Where("Name.Contains(@0) OR Code.Contains(@1)", value, value);
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
        private void GetPageInfo(Dictionary<string, StringValues> queries, ref int pageNumber, ref int pageSize)
        {
            StringValues pNumb;
            StringValues pSize;

            if (queries.TryGetValue(pageSizeKey, out pSize))
            {
                if (pSize.Count == 1)
                {
                    var value = -1;
                    Int32.TryParse(pSize[0], out value);
                    if (value > 0) pageSize = value;
                }
            }
            if (queries.TryGetValue(pageNumberKey, out pNumb))
            {
                if (pNumb.Count == 1)
                {
                    var value = -1;
                    Int32.TryParse(pNumb[0], out value);
                    if (value > 0) pageNumber = value;
                }
            }

        }

    }
}
