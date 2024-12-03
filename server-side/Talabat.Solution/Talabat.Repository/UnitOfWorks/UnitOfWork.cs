using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Repository.Layer.Data;

namespace Talabat.Repository.Layer.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;
        private Hashtable repositories;
        
        public UnitOfWork(StoreContext context)
        {
            this.context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(repositories == null)
            {
                repositories = new Hashtable();
            }
            string type = typeof(TEntity).Name;
            if (!repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(context);
                repositories.Add(type, repository);
            }
            return (IGenericRepository<TEntity>)repositories[type]!;
        }
    }
}
