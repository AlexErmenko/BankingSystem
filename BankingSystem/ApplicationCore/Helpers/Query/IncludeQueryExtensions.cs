using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ApplicationCore.Interfaces;

namespace ApplicationCore.Helpers.Query
{
  public static class IncludeQueryExtensions
  {
    public static IIncludeQuery<TEntity, TNewProperty> Include<TEntity, TPreviousProperty, TNewProperty>(this IIncludeQuery<TEntity, TPreviousProperty> query, Expression<Func<TEntity, TNewProperty>> selector)
    {
      query.Visitor.Visit(node: selector);

      var includeQuery = new IncludeQuery<TEntity, TNewProperty>(pathMap: query.PathMap);
      query.PathMap[key: includeQuery] = query.Visitor.Path;

      return includeQuery;
    }

    public static IIncludeQuery<TEntity, TNewProperty> ThenInclude<TEntity, TPreviousProperty, TNewProperty>(this IIncludeQuery<TEntity, TPreviousProperty> query, Expression<Func<TPreviousProperty, TNewProperty>> selector)
    {
      query.Visitor.Visit(node: selector);

      // If the visitor did not generated a path, return a new IncludeQuery with an unmodified PathMap.
      if(string.IsNullOrEmpty(value: query.Visitor.Path))
        return new IncludeQuery<TEntity, TNewProperty>(pathMap: query.PathMap);

      Dictionary<IIncludeQuery, string> pathMap = query.PathMap;
      string existingPath = pathMap[key: query];
      pathMap.Remove(key: query);

      var includeQuery = new IncludeQuery<TEntity, TNewProperty>(pathMap: query.PathMap);
      pathMap[key: includeQuery] = $"{existingPath}.{query.Visitor.Path}";

      return includeQuery;
    }

    public static IIncludeQuery<TEntity, TNewProperty> ThenInclude<TEntity, TPreviousProperty, TNewProperty>(this IIncludeQuery<TEntity, IEnumerable<TPreviousProperty>> query, Expression<Func<TPreviousProperty, TNewProperty>> selector)
    {
      query.Visitor.Visit(node: selector);

      // If the visitor did not generated a path, return a new IncludeQuery with an unmodified PathMap.
      if(string.IsNullOrEmpty(value: query.Visitor.Path))
        return new IncludeQuery<TEntity, TNewProperty>(pathMap: query.PathMap);

      Dictionary<IIncludeQuery, string> pathMap = query.PathMap;
      string existingPath = pathMap[key: query];
      pathMap.Remove(key: query);

      var includeQuery = new IncludeQuery<TEntity, TNewProperty>(pathMap: query.PathMap);
      pathMap[key: includeQuery] = $"{existingPath}.{query.Visitor.Path}";

      return includeQuery;
    }
  }
}
