using HotChocolate.Types;

namespace server.graphql.types
{
  public class Filter
  {
    public string Field { get; set; }
    public FilterOperator Operator { get; set; }
    public object Value { get; set; }
  }

  public sealed class FilterType : InputObjectType<Filter>
  {
    protected override void Configure(IInputObjectTypeDescriptor<Filter> descriptor)
    {
      descriptor.Description("The filter object.");

      // Properties
      descriptor.Field(f => f.Field)
        .Name("field")
        .Description("The name of the field to be filtered by.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Operator)
        .Name("operator")
        .Description("The filter operator type.")
        .Type<NonNullType<FilterOperatorType>>();

      descriptor.Field(f => f.Value)
        .Name("value")
        .Description("The optional filter value.")
        .Type<AnyType>();
    }
  }


  public sealed class FilterOutputType : ObjectType<Filter>
  {
    protected override void Configure(IObjectTypeDescriptor<Filter> descriptor)
    {
      descriptor.Description("The filter object.");

      // Properties
      descriptor.Field(f => f.Field)
        .Name("field")
        .Description("The name of the field to be filtered by.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Operator)
        .Name("operator")
        .Description("The filter operator type.")
        .Type<NonNullType<FilterOperatorType>>();

      descriptor.Field(f => f.Value)
        .Name("value")
        .Description("The optional filter value.")
        .Type<AnyType>();
    }
  }

}