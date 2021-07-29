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
        .Resolve(ctx => ctx.Service<ISystemSettingsModifiers>().SetSystemSettings(ctx.ArgumentValue<string>("key"), ctx.ArgumentValue<string>("value")));

      descriptor.Field("systemSettingsClear")
        .Description("Clear system settings.")
        .Argument("key", a => a.Type<NonNullType<StringType>>().Description("The settings key."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new[] { UserRole.ADMIN.ToString() })
        .Resolve(ctx => ctx.Service<ISystemSettingsModifiers>().ClearSystemSettings(ctx.ArgumentValue<string>("key")));
    }

  }
}