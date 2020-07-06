using System;
using shop_giay_server.models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace shop_giay_server._Repository
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {

        Task<T> GetById(int id, bool loadAllNavProperties = true);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, bool loadAllNavProperties = true);

        Task<T> Add(T entity);
        Task<IEnumerable<T>> Add(IEnumerable<T> entities);
        Task<T> Update(T entity);
        Task<bool> Remove(int id);

        Task<(IEnumerable<T> result, int totalRecords)> GetAllWithQuery(IQueryCollection query, bool loadAllNavProperties = true);
        Task<(IEnumerable<T> result, int totalRecords)> GetAll(Dictionary<string, StringValues> queries, bool loadAllNavProperties = true);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

        Task<bool> ExistWhere(Expression<Func<T, bool>> predicate);
    }
}