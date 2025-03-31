﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rimovie.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> InsertAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(int id);
    }
}
