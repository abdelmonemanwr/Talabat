using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>>? Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; } = new ();

        public Expression<Func<T, object>>? OrderByAscendingExpression { get; set; }

        public Expression<Func<T, object>>? OrderByDescendingExpression { get; set; }
        
        public int SkippedAmount { get; set; }
        
        public int TakenAmount { get; set; }

        public bool IsPaginationEnabled { get; set; } = false;


        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>>? Criteria = null) => 
            this.Criteria = Criteria;

        public void AddOrderByAscendingExpression(Expression<Func<T, object>> OrderByAscendingExpression) =>
            this.OrderByAscendingExpression = OrderByAscendingExpression;
        
        public void AddOrderByDescendingExpression(Expression<Func<T, object>> OrderByDescendingExpression) =>
            this.OrderByDescendingExpression = OrderByDescendingExpression;

        public void AddPagination(int SkippedAmount, int TakenAmount)
        {
            this.SkippedAmount = SkippedAmount;
            this.TakenAmount = TakenAmount;
            IsPaginationEnabled = true;
        }

    }
}