using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using server.extensions;
using server.graphql.models;
using server.graphql.types;

namespace server.graphql.extensions
{
  public static class GraphqlExtensions
  {
    private static readonly string REGEX_UTC_DATE = "^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[01]|0[1-9]|[12][0-9])T(2[0-3]|[01][0-9]):([0-5][0-9]):([0-5][0-9])(\\.[0-9]+)?\\.([0-9]{3})(Z)?$";

    public static Key GetKey(this IResolverContext context, int id)
    {
      var info = GetKeyInfo(context);

      return new Key
      {
        Id = id,
        Skip = info.Skip,
        Take = info.Take,
        Filter = info.Filter,
        Sort = info.Sort
      };
    }

    public static KeyInfo GetKeyInfo(this IResolverContext context)
    {
      // Extract filter value
      object filter = null;
      context.ScopedContextData.TryGetValue("filter", out filter);

      // Extract restriction value
      object restriction = null;
      context.ScopedContextData.TryGetValue("restriction", out restriction);

      // Extract sort value
      object sort = null;
      context.ScopedContextData.TryGetValue("sort", out sort);

      // Remove "filter" and "sort" keys from the scoped context data
      context.ScopedContextData = context.ScopedContextData.Keys
        .Where(k => k != "filter" && k != "restriction" && k != "sort")
        .ToImmutableDictionary(k => k, v => context.ScopedContextData.GetValueOrDefault(v));

      return new KeyInfo
      {
        Skip = context.Argument<int?>("skip"),
        Take = context.Argument<int?>("take"),
        Filter = (FilterRoot)filter,
        Sort = (List<Sort>)sort
      };
    }

    public static FilterRoot ToComplexFilter(this Filter filter)
    {
      return new FilterRoot
      {
        Logic = FilterLogic.AND,
        Filters = new List<FilterGroup>
        {
          new FilterGroup
          {
            Logic = FilterLogic.AND,
            Filters = new List<Filter> { filter }
          }
        }
      };
    }

    public static FilterRoot ToComplexFilter(this FilterGroup filter)
    {
      return new FilterRoot
      {
        Logic = FilterLogic.AND,
        Filters = new List<FilterGroup> { filter }
      };
    }

    public static FilterRoot ConvertFilterValues(this FilterRoot filter)
    {
      foreach (var rootFilter in filter.Filters)
        foreach (var f in rootFilter.Filters)
        {
          // Convert string to DateTimeOffset
          if (f.Value != null && Regex.IsMatch(f.Value?.ToString(), REGEX_UTC_DATE))
          {
            f.Value = f.Value?.ToString().ToDateTimeOffset();
          }
        }

      return filter;
    }

    public static IObjectFieldDescriptor UsePagination<TType, T>(this IObjectFieldDescriptor descriptor)
      where TType : ObjectType
      where T : class
    {
      return descriptor
        .Argument("skip", a => a.Type<IntType>().Description("The number of the records to be skipped before fetching."))
        .Argument("take", a => a.Type<IntType>().Description("The number of the records to fetch."))
        .Type<NonNullType<PaginationType<TType, T>>>();
    }

  }
}