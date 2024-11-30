using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Specifications;

namespace Talabat.Repository.Layer.Data
{
    public class SpecificationEvaluator<T> where T: BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> EntryQuery, ISpecification<T> Specifications)
        {
            if (Specifications.Criteria != null)
                EntryQuery = EntryQuery.Where(Specifications.Criteria);
            
            if (Specifications.OrderByAscendingExpression != null)
                EntryQuery = EntryQuery.OrderBy(Specifications.OrderByAscendingExpression);
            
            if (Specifications.OrderByDescendingExpression != null)
                EntryQuery = EntryQuery.OrderByDescending(Specifications.OrderByDescendingExpression);

            if (Specifications.IsPaginationEnabled)
                EntryQuery = EntryQuery.Skip(Specifications.SkippedAmount).Take(Specifications.TakenAmount);

            return Specifications.Includes.Aggregate(EntryQuery, (CurrentQuery, CurrentIncludeExpression)
                => CurrentQuery.Include(CurrentIncludeExpression));
        }
    }
}