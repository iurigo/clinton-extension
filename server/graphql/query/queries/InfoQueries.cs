using HotChocolate.Types;
using server.graphql.query.resolvers;

namespace server.graphql.query.queries
{
  public static class InfoQueries
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("version")
        .Description("The application version.")
        .Type<NonNullType<StringType>>()
        .Resolver(ctx => ctx.Service<IInfoResolvers>().GetVersion());

      descriptor.Field("userId")
        .Description("The current user id.")
        .Type<NonNullType<IntType>>()
        .Resolver(ctx => ctx.Service<IInfoResolvers>().GetCurrentUserId());

      descriptor.Field("fullName")
        .Description("The full name of the current user.")
        .Type<NonNullType<StringType>>()
        .Resolver(ctx => ctx.Service<IInfoResolvers>().GetCurrentUserFullName());

      descriptor.Field("isAdmin")
        .Description("Indicates if current user is admin.")
        .Type<NonNullType<BooleanType>>()
        .Resolver(ctx => ctx.Service<IInfoResolvers>().IsAdmin());

      descriptor.Field("isActive")
        .Description("Indicates if current user's account is active.")
        .Type<NonNullType<BooleanType>>()
        .Resolver(ctx => ctx.Service<IInfoResolvers>().IsActive());
    }

  }
}