﻿using System;
using shop_giay_server.models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace shop_giay_server._Repository
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {

        Task<T> GetById(int id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Remove(int id);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

    }
}