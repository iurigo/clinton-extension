using HotChocolate.Types;
using server.database.enums;
using server.graphql.query.resolvers;

namespace server.graphql.query.queries
{
  public class SystemSettingsQueries
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("systemSettings")
        .Description("Get the system settings.")
        .Argument("key", a => a.Type<NonNullType<StringType>>().Description("The settings' key value."))
        .Type<StringType>()
        .Authorize(new[] { UserRole.ADMIN.ToString() })
        .Resolve(ctx => ctx.Service<ISystemSettingsResolvers>().GetSystemSettings(ctx.ArgumentValue<string>("key")));
    }

  }
}