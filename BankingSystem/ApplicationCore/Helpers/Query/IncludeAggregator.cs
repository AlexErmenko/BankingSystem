using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ApplicationCore.Interfaces;

namespace ApplicationCore.Helpers.Query
{
  public class IncludeAggregator<TEntity>
  {
    public IncludeQuery<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> selector)
    {
      var visitor = new IncludeVisitor();
      visitor.Visit(node: selector);

      var pathMap = new Dictionary<IIncludeQuery, string>();
      var query = new IncludeQuery<TEntity, TProperty>(pathMap: pathMap);

      if(!string.IsNullOrEmpty(value: visitor.Path))
        pathMap[key: query] = visitor.Path;

      return query;
    }
  }
}
