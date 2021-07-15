using System;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using server.database.enums;
using server.extensions;
using server.graphql.mutation.modifiers;
using server.graphql.mutation.types.user;
using server.graphql.query.types.user;

namespace server.graphql.mutation.mutations
{
  public static class UserMutations
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("userCreate")
        .Description("Create a new user.")
        .Argument("user", a => a.Type<NonNullType<UserCreateInputType>>().Description("The user input model."))
        .Type<NonNullType<UserType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString()})
        .Resolver(ctx => ctx.Service<IUserModifiers>().CreateUser(ctx.Argument<UserCreateInput>("user")));

      descriptor.Field("userUpdate")
        .Description("Update an existing user.")
        .Argument("user", a => a.Type<NonNullType<UserUpdateInputType>>().Description("The user input model."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString()})
        .Resolver(ctx => ctx.Service<IUserModifiers>().UpdateUser(ctx.Argument<UserUpdateInput>("user")));

      descriptor.Field("userDelete")
        .Description("Delete an existing user.")
        .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The user id."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString()})
        .Resolver(ctx => ctx.Service<IUserModifiers>().DeleteUser(ctx.Argument<int>("id")));

      descriptor.Field("userSetPassword")
        .Description("Reset user's password.")
        .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The user id."))
        .Argument("password", a => a.Type<NonNullType<StringType>>().Description("The new password."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString()})
        .Resolver(ctx => ctx.Service<IUserModifiers>().SetPassword(ctx.Argument<int>("id"), ctx.Argument<string>("password")));

      descriptor.Field("userSetOwnPassword")
        .Description("Reset user's own password.")
        .Argument("password", a => a.Type<NonNullType<StringType>>().Description("The new password."))
        .Type<NonNullType<BooleanType>>()
        // Authorization inside the resolver
        .Resolver(ctx =>
        {
          var userId = ctx.Service<IHttpContextAccessor>().HttpContext.GetUserId();
          return ctx.Service<IUserModifiers>().SetPassword(userId ?? new int(), ctx.Argument<string>("password"));
        });
    }

  }
}