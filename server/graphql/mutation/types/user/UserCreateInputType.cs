using HotChocolate.Types;
using server.database.enums;
using server.graphql.query.types.user;

namespace server.graphql.mutation.types.user
{
  public class UserCreateInput
  {
    // Properties
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public string FullName { get; set; }
  }


  public class UserCreateInputType : InputObjectType<UserCreateInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<UserCreateInput> descriptor)
    {
      descriptor.Description("The new user input type.");

      // Properties
      descriptor.Field(f => f.Username)
        .Name("username")
        .Description("The username of the user.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Password)
        .Name("password")
        .Description("The password of the user.")
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