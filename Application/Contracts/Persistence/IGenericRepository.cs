﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> Get(int id);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> Add(T entity);
        Task<bool> Exists(int id);
        void Update(T entity);
        void Delete(T entity);
    }
}
