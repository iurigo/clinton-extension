using System.Collections.Generic;
using HotChocolate.Types;

namespace server.graphql.types
{
  public class FilterGroup
  {
    public FilterLogic Logic { get; set; }
    public IEnumerable<Filter> Filters { get; set; }
  }


  public sealed class FilterGroupType : InputObjectType<FilterGroup>
  {
    protected override void Configure(IInputObjectTypeDescriptor<FilterGroup> descriptor)
    {
      descriptor.Description("The filter group object.");

      // Properties
      descriptor.Field(f => f.Logic)
        .Name("logic")
        .Description("The underling filters processing logic.")
        .Type<NonNullType<FilterLogicType>>();

      descriptor.Field(f => f.Filters)
        .Name("filters")
        .Description("The underling filters.")
        .Type<NonNullType<ListType<NonNullType<FilterType>>>>();
    }
  }


  public sealed class FilterGroupOutputType : ObjectType<FilterGroup>
  {
    protected override void Configure(IObjectTypeDescriptor<FilterGroup> descriptor)
    {
      descriptor.Description("The filter group object.");

      // Properties
      descriptor.Field(f => f.Logic)
        .Name("logic")
        .Description("The underling filters processing logic.")
        .Type<NonNullType<FilterLogicType>>();

      descriptor.Field(f => f.Filters)
        .Name("filters")
        .Description("The underling filters.")
        .Type<NonNullType<ListType<NonNullType<FilterOutputType>>>>();
    }
  }

}