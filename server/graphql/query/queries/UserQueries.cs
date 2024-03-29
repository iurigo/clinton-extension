using System.Threading;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using server.database.models;
using server.graphql.extensions;
using server.graphql.query.resolvers;
using server.graphql.query.types.user;

namespace server.graphql.query.queries
{
  public class UserQueries
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("users")
        .Description("The list of users.")
        .Type<ListType<UserType>>()
        .Authorize()
        .Resolve(ctx => ctx.Service<IUserResolvers>().GetUsers(ctx.GetKeyInfo()));

      descriptor.Field("user")
        .Description("Get the user by id.")
        .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The unique identifier."))
        .Type<UserType>()
        .Authorize()
        .Resolve(ctx =>
        {
          return ctx.BatchDataLoader<int, User>(
            (keys, _) => ctx.Service<IUserResolvers>().GetUserById(keys),
            nameof(IUserResolvers.GetUserById)
          ).LoadAsync(ctx.ArgumentValue<int>("id"), CancellationToken.None);
        });
    }

  }
}