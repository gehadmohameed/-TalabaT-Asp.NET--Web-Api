using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Core.Entites;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public static class SpecifiacationEvalutor<T> where T :BaseEntity
    {
     public static IQueryable<T> GetQuery(IQueryable<T> Inputquery, ISpecifications<T> Spec)
        {
            var Query = Inputquery;
            if (Spec.Criteria is not null )
            {
                Query = Query.Where(Spec .Criteria);
              
            }
            if( Spec.OrderBy is not null ) {
                
                Query = Query.OrderBy(Spec .OrderBy);
            }
            if (Spec.OrderByDescending is not null)
            {

                Query = Query.OrderByDescending(Spec.OrderByDescending);
            }
            if (Spec.IsPaginationEnabled)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            return Query;

        }

    }
}
