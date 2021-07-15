using System.Collections.Generic;
using HotChocolate.Execution;
using HotChocolate.Types;
using server.graphql.extensions;
using server.graphql.types;

namespace server.graphql.directives
{
  public class FilterDirective : DirectiveType
  {
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
      descriptor.Name("filter");
      descriptor.Location(DirectiveLocation.Field);
      descriptor.Argument("simple").Type<FilterType>().Description("Filter by single field.");
      descriptor.Argument("filter").Type<FilterGroupType>().Description("Filter by the list of fields.");
      descriptor.Argument("complex").Type<FilterRootType>().Description("Complex filter for the most complex scenarios.");

      descriptor.Use(next => async context =>
      {
        var filters = new List<FilterRoot>();
        try { filters.Add(context.Directive.GetArgument<Filter>("simple").ToComplexFilter().ConvertFilterValues()); } catch { }
        try { filters.Add(context.Directive.GetArgument<FilterGroup>("filter").ToComplexFilter().ConvertFilterValues()); } catch { }
        try { filters.Add(context.Directive.GetArgument<FilterRoot>("complex").ConvertFilterValues()); } catch { }

        // Validate
        if (filters.Count == 0) { throw new QueryException("At least one filter type should be specified."); }
        if (filters.Count != 1) { throw new QueryException("Only one filter type can be applied at once."); }

        // Set filter value
        context.ScopedContextData = context.ScopedContextData.Add("filter", filters[0]);

        await next.Invoke(context).ConfigureAwait(false);
      });
    }

  }
}