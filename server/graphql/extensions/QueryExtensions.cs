using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using server.graphql.models;
using server.graphql.types;

namespace server.graphql.extensions
{
  public static class QueryExtensions
  {
    public static async Task<(List<T> Data, int TotalCount)> ToPageableQuery<T>(this IQueryable<T> query, KeyInfo info) where T : class
    {
      // Set default sorting if needed
      if (info.Sort == null || !info.Sort.Any())
      {
        info.Sort = new List<Sort> { new Sort { Direction = SortDirection.ASC, Field = "CreatedAt" } };
      };

      // Apply the filter
      if (info.Filter != null) { query = query.Filter(info.Filter); }

      // Get total number of records
      var total = await query.CountAsync();

      // Apply the sorting
      query = query.Sort(info.Sort);

      // Pagination
      if (info.Skip.HasValue && info.Take.HasValue) 
      { 
        query = query
          .Skip(info.Skip.Value)
          .Take(info.Take.Value);
      }

      // Get the data
      var data = await query.AsNoTracking().ToListAsync();

      // Return the result
      return (data, total);
    }

    public static async Task<List<T>> ToQuery<T>(this IQueryable<T> query, KeyInfo info, string defaultSortColumn = "CreatedAt") where T : class
    {
      // Set default sorting if needed
      if (info.Sort == null || !info.Sort.Any())
      {
        info.Sort = new List<Sort> { new Sort { Direction = SortDirection.ASC, Field = defaultSortColumn } };
      };

      // Apply the filter
      if (info.Filter != null) { query = query.Filter(info.Filter); }

      // Apply the sorting
      query = query.Sort(info.Sort);

      // Get the data
      var data = await query.AsNoTracking().ToListAsync();

      // Return the result
      return data;
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> query, FilterRoot filter)
    {
      // Validate input parameters
      if (filter == null || !filter.Filters.Any()) { return query; }

      // Revalidate after filter clean up
      if (!filter.Filters.Any() || !filter.Filters.SelectMany(f => f.Filters).Any()) { return query; }

      // Input parameter (w =>)
      var w = Expression.Parameter(typeof(T), "w");

      // Build expressions
      Expression predicateBody = null;
      foreach (var globalFilter in filter.Filters)
      {
        Expression subPredicateBody = null;
        foreach (var f in globalFilter.Filters)
        {
          // var field = typeof(T).GetProperty(f.Field.ToUpperFirstChar());
          var field = typeof(T).GetProperties().FirstOrDefault(p =>  p.Name.ToLower() == f.Field.ToLower());
          if (field == null) { throw new QueryException($"The field \"{f.Field}\" was not found."); }
          var left = Expression.Property(w, field);
          var expression = left.CompareExpression<T>(f, field.PropertyType);

          // Combine sub-filter expression parts
          if (subPredicateBody == null)
          {
            subPredicateBody = expression;
          }
          else
          {
            subPredicateBody = (globalFilter.Logic == FilterLogic.AND) ?
              Expression.AndAlso(subPredicateBody, expression) :
              Expression.OrElse(subPredicateBody, expression);
          }
        }
        // Combine global filter expression parts
        if (predicateBody == null)
        {
          predicateBody = subPredicateBody;
        }
        else
        {
          predicateBody = (filter.Logic == FilterLogic.AND) ?
            Expression.AndAlso(predicateBody, subPredicateBody) :
            Expression.OrElse(predicateBody, subPredicateBody);
        }
      }

      // Result
      return query.Provider.CreateQuery<T>(Expression.Call(
        typeof(Queryable),
        "Where",
        new Type[] { query.ElementType },
        query.Expression,
        Expression.Lambda<Func<T, bool>>(predicateBody, w)
      ));
    }

    public static Expression CompareExpression<T>(this Expression left, Filter filter, Type fieldType)
    {
      // Get right part of the expression
      Expression right = null;
      switch (filter.Operator)
      {
        case FilterOperator.IS_NULL:
        case FilterOperator.IS_NOT_NULL:
          right = Expression.Constant(null);
          break;

        case FilterOperator.IS_EMPTY:
        case FilterOperator.IS_NOT_EMPTY:
          right = Expression.Constant(string.Empty);
          break;

        default:
          left = Expression.Convert(Expression.Convert(left, typeof(object)), fieldType);
          right = Expression.Convert(Expression.Constant(filter.Value), fieldType);
          break;
      }

      // Combine left and right parts into one expression
      MethodInfo method = null;
      switch (filter.Operator)
      {
        case FilterOperator.EQUAL: return Expression.Equal(left, right);
        case FilterOperator.NOT_EQUAL: return Expression.NotEqual(left, right);
        case FilterOperator.IS_NULL: return Expression.Equal(left, right);
        case FilterOperator.IS_NOT_NULL: return Expression.NotEqual(left, right);
        case FilterOperator.LESS_THAN: return Expression.LessThan(left, right);
        case FilterOperator.LESS_THAN_OR_EQUAL: return Expression.LessThanOrEqual(left, right);
        case FilterOperator.GREATER_THAN: return Expression.GreaterThan(left, right);
        case FilterOperator.GREATER_THAN_OR_EQUAL: return Expression.GreaterThanOrEqual(left, right);
        case FilterOperator.STARTS_WITH:
          method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
          return Expression.Call(left, method, right);

        case FilterOperator.ENDS_WITH:
          method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
          return Expression.Call(left, method, right);

        case FilterOperator.CONTAINS:
          method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
          return Expression.Call(left, method, right);

        case FilterOperator.DOES_NOT_CONTAIN:
          method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
          return Expression.Not(Expression.Call(left, method, right));

        case FilterOperator.IS_EMPTY: return Expression.Equal(left, right);
        case FilterOperator.IS_NOT_EMPTY: return Expression.Not(Expression.Equal(left, right));

        default: throw new NotImplementedException(nameof(filter.Operator));
      }
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query, List<Sort> sort)
    {
      // Validate input parameters
      if (sort == null || !sort.Any()) { return query; }

      // Expression parameter (o =>) and the property (o.FieldName)
      var o = Expression.Parameter(typeof(T), "o");
      var propertyExp = o.GetPropertyExpression<T>(sort[0].Field);

      // Apply first level sorting
      query = sort[0].Direction == SortDirection.ASC ?
        query.OrderBy(Expression.Lambda<Func<T, object>>(propertyExp, o)) :
        query.OrderByDescending(Expression.Lambda<Func<T, object>>(propertyExp, o));

      // Apply the next levels
      foreach (var sorting in sort.Skip(1))
      {
        propertyExp = o.GetPropertyExpression<T>(sorting.Field);
        query = sorting.Direction == SortDirection.ASC ?
          ((IOrderedQueryable<T>)query).ThenBy(Expression.Lambda<Func<T, object>>(propertyExp, o)) :
          ((IOrderedQueryable<T>)query).ThenByDescending(Expression.Lambda<Func<T, object>>(propertyExp, o));
      }

      // Return the sorted query
      return query;
    }

    private static UnaryExpression GetPropertyExpression<T>(this ParameterExpression i, string fieldName)
    {
      var property = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == fieldName.ToLower());
      if (property == null) { throw new ArgumentException($"Invalid property name: {fieldName}"); }
      return Expression.Convert(Expression.Property(i, property), typeof(object));
    }

  }
}