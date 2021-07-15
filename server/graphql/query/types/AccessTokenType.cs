using HotChocolate.Types;
using server.database.models;
using server.services.jwt_service.models;

namespace server.graphql.query.types
{
  public sealed class AccessTokenType : ObjectType<AccessToken>
  {
    protected override void Configure(IObjectTypeDescriptor<AccessToken> descriptor)
    {
      descriptor.Description("The access token.");

      // Properties
      descriptor.Field(f => f.Token)
        .Name("token")
        .Description("The access token.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.RefreshToken)
        .Name("refreshToken")
        .Description("The refresh token is used to get a new access token.")
        .Type<NonNullType<StringType>>();
    }

  }
}