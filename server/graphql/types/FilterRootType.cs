using System.Collections.Generic;
using HotChocolate.Types;

namespace server.graphql.types
{
  public class FilterRoot
  {
    public FilterLogic Logic { get; set; }
    public List<FilterGroup> Filters { get; set; }
  }


  public sealed class FilterRootType : InputObjectType<FilterRoot>
  {
    protected override void Configure(IInputObjectTypeDescriptor<FilterRoot> descriptor)
    {
      descriptor.Description("The filter root object.");

      // Properties
      descriptor.Field(f => f.Logic)
        .Name("logic")
        .Description("The underling filter groups processing logic.")
        .Type<NonNullType<FilterLogicType>>();

      descriptor.Field(f => f.Filters)
        .Name("filters")
        .Description("The underling filter groups.")
        .Type<NonNullType<ListType<NonNullType<FilterGroupType>>>>();
    }
  }


  public sealed class FilterRootOutputType : ObjectType<FilterRoot>
  {
    protected override void Configure(IObjectTypeDescriptor<FilterRoot> descriptor)
    {
      descriptor.Description("The filter root object.");

      // Properties
      descriptor.Field(f => f.Logic)
        .Name("logic")
        .Description("The underling filter groups processing logic.")
        .Type<NonNullType<FilterLogicType>>();

      descriptor.Field(f => f.Filters)
        .Name("filters")
        .Description("The underling filter groups.")
        .Type<NonNullType<ListType<NonNullType<FilterGroupOutputType>>>>();
    }
  }

}