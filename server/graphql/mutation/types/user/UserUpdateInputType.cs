using HotChocolate.Types;
using server.database.enums;
using server.database.models;
using server.graphql.query.types;
using server.graphql.query.types.user;
using System;

namespace server.graphql.mutation.types.user
{
  public class UserUpdateInput
  {
    // Key
    public int Id { get; set; }
    
    // Properties
    public string Username { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
  }


  public class UserUpdateInputType : InputObjectType<UserUpdateInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<UserUpdateInput> descriptor)
    {
      descriptor.Description("The update user input type.");

      // Key
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("Unique identifier.")
        .Type<NonNullType<IntType>>();

      // Properties
      descriptor.Field(f => f.Username)
        .Name("username")
        .Description("The username of the user.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.FullName)
        .Name("fullName")
        .Description("The full name of the user.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Role)
        .Name("role")
        .Description("The role of the user.")
        .Type<NonNullType<UserRoleType>>();

      descriptor.Field(f => f.Status)
        .Name("status")
        .Description("Indicates if the user is active.")
        .Type<NonNullType<UserStatusType>>();
    }

  }
}