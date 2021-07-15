using HotChocolate.Types;

namespace server.graphql.types
{
  public class Sort
  {
    public SortDirection Direction { get; set; }
    public string Field { get; set; }
  }


  public sealed class SortType : InputObjectType<Sort>
  {
    protected override void Configure(IInputObjectTypeDescriptor<Sort> descriptor)
    {
      descriptor.Description("The sorting object.");

      // Properties
      descriptor.Field(f => f.Direction)
        .Name("direction")
        .Description("The sorting direction.")
        .Type<NonNullType<SortDirectionType>>();

      descriptor.Field(f => f.Field)
        .Name("field")
        .Description("The sorting field name.")
        .Type<NonNullType<StringType>>();
    }

  }
}