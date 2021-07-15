using System.Collections.Generic;
using HotChocolate.Execution;
using HotChocolate.Types;
using server.graphql.types;

namespace server.graphql.directives
{
  public class SortDirective : DirectiveType
  {
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
      descriptor.Name("sort");
      descriptor.Location(DirectiveLocation.Field);
      descriptor.Argument("simple").Type<SortType>().Description("Simple sorting by one filed.");
      descriptor.Argument("complex").Type<ListType<SortType>>().Description("Complex sorting by many fields.");

      descriptor.Use(next => async context =>
      {
        var sorting = new List<Sort>();
        var counter = 0;
        try { sorting.Add(context.Directive.GetArgument<Sort>("simple")); counter++; } catch { }
        try { sorting.AddRange(context.Directive.GetArgument<List<Sort>>("complex")); counter++; } catch { }

        // Validate
        if (counter == 0) { throw new QueryException("At least one sorting type should be specified."); }
        if (counter != 1) { throw new QueryException("Only one sorting type can be applied at once."); }

        // Set filter value
        context.ScopedContextData = context.ScopedContextData.Add("sort", sorting);

        await next.Invoke(context).ConfigureAwait(false);
      });
    }

  }
}