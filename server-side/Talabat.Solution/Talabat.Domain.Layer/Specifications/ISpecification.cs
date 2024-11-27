using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; }

        public Expression<Func<T, object>>? OrderByAscendingExpression { get; set; }

        public Expression<Func<T, object>>? OrderByDescendingExpression { get; set; }

        public int SkippedAmount { get; set; }

        public int TakenAmount { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
