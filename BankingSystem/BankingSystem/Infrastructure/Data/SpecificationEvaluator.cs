using System;
using System.Linq;

using ApplicationCore.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class SpecificationEvaluator<T> where T : class
	{
		public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
		{
			var query = inputQuery;

			// modify the IQueryable using the specification's criteria expression
			if(specification.Criteria != null) query = query.Where(predicate: specification.Criteria);

			// Includes all expression-based includes
			query = specification.Includes.Aggregate(seed: query, func: (current, include) => current.Include(navigationPropertyPath: include));

			// Include any string-based include statements
			query = specification.IncludeStrings.Aggregate(seed: query, func: (current, include) => current.Include(navigationPropertyPath: include));

			// Apply ordering if expressions are set
			if(specification.OrderBy != null) query = query.OrderBy(keySelector: specification.OrderBy);
			else if(specification.OrderByDescending != null)
				query = query.OrderByDescending(keySelector: specification.OrderByDescending);

			if(specification.GroupBy != null) query = query.GroupBy(keySelector: specification.GroupBy).SelectMany(selector: x => x);

			// Apply paging if enabled
			if(specification.IsPagingEnabled) query = query.Skip(count: specification.Skip).Take(count: specification.Take);
			return query;
		}
	}
}
