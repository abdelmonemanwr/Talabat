using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.Specifications;
using Talabat.Repository.Layer.Data;

namespace Talabat.Repository.Layer
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context) => 
            _context = context;

        public async Task<IEnumerable<T>> GetAllAsync() => 
            await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specifications) => 
            await ApplySpecifications(specifications).ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => 
            await _context.Set<T>().FindAsync(id);

        public async Task<T?> GetByIdAsync(ISpecification<T> specifications) =>
            await ApplySpecifications(specifications).FirstOrDefaultAsync();
        
        private IQueryable<T> ApplySpecifications(ISpecification<T> specifications)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specifications);
        }
    }
}