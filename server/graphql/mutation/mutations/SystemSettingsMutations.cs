using HotChocolate.Types;
using server.database.enums;
using server.graphql.mutation.modifiers;

namespace server.graphql.mutation.mutations
{
  public static class SystemSettingsMutations
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("systemSettingsSet")
        .Description("Set system settings.")
        .Argument("key", a => a.Type<NonNullType<StringType>>().Description("The settings key."))
        .Argument("value", a => a.Type<StringType>().Description("The settings value."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new[] { UserRole.ADMIN.ToString() })
        .Resolver(ctx => ctx.Service<ISystemSettingsModifiers>().SetSystemSettings(ctx.Argument<string>("key"), ctx.Argument<string>("value")));

      descriptor.Field("systemSettingsClear")
        .Description("Clear system settings.")
        .Argument("key", a => a.Type<NonNullType<StringType>>().Description("The settings key."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new[] { UserRole.ADMIN.ToString() })
        .Resolver(ctx => ctx.Service<ISystemSettingsModifiers>().ClearSystemSettings(ctx.Argument<string>("key")));
    }

  }
}