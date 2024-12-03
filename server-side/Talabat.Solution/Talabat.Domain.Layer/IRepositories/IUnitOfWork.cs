using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.IRepositories
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity: BaseEntity ;
        Task<int> SaveChangesAsync();
    }
}
