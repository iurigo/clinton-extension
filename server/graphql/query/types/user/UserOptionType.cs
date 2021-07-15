using System;
using HotChocolate.Types;
using server.graphql.query.types;

namespace server.graphql.types
{
  public class UserOption
  {
    public int Id { get; set; }
    public string FullName { get; set; }
  }


  public class UserOptionType : ObjectType<UserOption>
  {
    protected override void Configure(IObjectTypeDescriptor<UserOption> descriptor)
    {
      descriptor.Description("A generic data option object.");

      // Properties
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("The unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.FullName)
        .Name("fullName")
        .Description("The text representation of the option.")
        .Type<StringType>();
    }

  }
}