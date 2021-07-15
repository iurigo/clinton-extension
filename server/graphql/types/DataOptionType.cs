using System;
using HotChocolate.Types;
using server.database.enums;
using server.graphql.query.types;

namespace server.graphql.types
{
  public class DataOptionOptions
  {
    public EventType Id { get; set; }
    public string Value { get; set; }
  }


  public class DataOptionOptionsType : ObjectType<DataOptionOptions>
  {
    protected override void Configure(IObjectTypeDescriptor<DataOptionOptions> descriptor)
    {
      descriptor.Description("A generic data option object.");

      // Properties
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("The unique identifier.")
        .Type<NonNullType<EventTypeType>>();

      descriptor.Field(f => f.Value)
        .Name("value")
        .Description("The text representation of the option.")
        .Type<StringType>();
    }

  }
}