using System.Threading;
using HotChocolate.Types;
using server.database.models;
using server.graphql.extensions;
using server.graphql.models;
using server.graphql.query.resolvers;

namespace server.graphql.query.types.user
{
  public class UserType : ObjectType<User>
  {
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
      descriptor.Description("User.");

      // Public fields
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("Unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.Username)
        .Name("username")
        .Description("Username.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.FullName)
        .Name("fullName")
        .Description("The full name of the user.")
        .Type<NonNullType<StringType>>();


      descriptor.Field(f => f.Status)
        .Name("status")
        .Description("Indicates if the user's account is active.")
        .Type<NonNullType<UserStatusType>>();

      descriptor.Field(f => f.Role)
        .Name("role")
        .Description("User's Role")
        .Type<NonNullType<UserRoleType>>();

      descriptor.Field("systemLogs")
        .Description("The list of actions made by user")
        .Type<NonNullType<ListType<NonNullType<SystemLogType>>>>()
        .Resolve(ctx =>
        {
          return ctx.GroupDataLoader<Key, SystemLog>(
            (keys, _) => ctx.Service<ISystemLogResolvers>().GetSystemLogByUserId(keys),
            nameof(ISystemLogResolvers.GetSystemLogByUserId)
          ).LoadAsync(ctx.GetKey(ctx.Parent<User>().Id), CancellationToken.None);
        });

      descriptor.Field(f => f.CreatedAt)
        .Name("createdAt")
        .Description("Shows when the record was created.")
        .Type<NonNullType<DateTimeType>>();

      descriptor.Field(f => f.UpdatedAt)
        .Name("updatedAt")
        .Description("Shows when the record was last updated.")
        .Type<NonNullType<DateTimeType>>();


      // Hidden fields
      descriptor.Field(f => f.PasswordHash).Ignore();
      descriptor.Field(f => f.PasswordSalt).Ignore();
      descriptor.Field(f => f.DeletedAt).Ignore();
    }

  }
}