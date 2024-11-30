﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Specifications;

namespace Talabat.Domain.Layer.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specifications); // For including extra items

        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(ISpecification<T> specifications);

        Task<int> GetCountAsync(ISpecification<T> specifications);
    }
}
