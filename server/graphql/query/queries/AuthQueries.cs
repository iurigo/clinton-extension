using HotChocolate.Types;
using server.graphql.query.resolvers;
using server.graphql.query.types;

namespace server.graphql.query.queries
{
  public static class AuthQueries
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("accessToken")
        .Description("Get the access-token by username and password.")
        .Argument("username", a => a.Type<NonNullType<StringType>>().Description("The username"))
        .Argument("password", a => a.Type<NonNullType<StringType>>().Description("The password"))
        .Type<NonNullType<AccessTokenType>>()
        .Resolver(ctx =>
        {
          return ctx.Service<IAccessTokenResolvers>()
            .GetByUsernameAndPassword(ctx.Argument<string>("username"), ctx.Argument<string>("password"));
        });

      descriptor.Field("refreshToken")
        .Description("Get the access-token by refresh-token.")
        .Argument("token", a => a.Type<NonNullType<StringType>>().Description("The refresh-token"))
        .Type<NonNullType<AccessTokenType>>()
        .Resolver(ctx =>
        {
          return ctx.Service<IAccessTokenResolvers>()
            .GetAccessTokenByRefreshToken(ctx.Argument<string>("token"));
        });
    }
  }
}