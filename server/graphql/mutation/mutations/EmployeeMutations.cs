using System.Collections.Generic;
using HotChocolate.Types;
using server.database.enums;
using server.graphql.mutation.modifiers;
using server.graphql.mutation.types.employee;
using server.graphql.query.types.employee;

namespace server.graphql.mutation.mutations
{
  public static class EmployeeMutations
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("employeeCreate")
        .Description("Create a new employee.")
        .Argument("employee", a => a.Type<NonNullType<EmployeeInputType>>().Description("The employee input model."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString(), UserRole.USER.ToString()})
        .Resolve(ctx => ctx.Service<IEmployeeModifiers>().CreateEmployee(ctx.ArgumentValue<EmployeeInput>("employee")));

      descriptor.Field("employeeMultipleCreate")
        .Description("Create a new employees.")
        .Argument("employees", a => a.Type<ListType<NonNullType<EmployeeInputType>>>().Description("The employees input model."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString(), UserRole.USER.ToString()})
        .Resolve(ctx => ctx.Service<IEmployeeModifiers>().CreateMultipleEmployees(ctx.ArgumentValue<List<EmployeeInput>>("employees")));

      descriptor.Field("employeeMultipleUpdate")
        .Description("Update existing employees.")
        .Argument("employees", a => a.Type<ListType<NonNullType<EmployeeInputType>>>().Description("The employees input model."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString(), UserRole.USER.ToString()})
        .Resolve(ctx => ctx.Service<IEmployeeModifiers>().UpdateMultipleEmployees(ctx.ArgumentValue<List<EmployeeInput>>("employees")));

      descriptor.Field("employeeUpdate")
        .Description("Update an existing employee.")
        .Argument("employee", a => a.Type<NonNullType<EmployeeInputType>>().Description("The employee input model."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString(), UserRole.USER.ToString()})
        .Resolve(ctx => ctx.Service<IEmployeeModifiers>().UpdateEmployee(ctx.ArgumentValue<EmployeeInput>("employee")));

      descriptor.Field("employeeDelete")
        .Description("Delete an existing employee.")
        .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The employee id."))
        .Type<NonNullType<BooleanType>>()
        .Authorize(new [] {UserRole.ADMIN.ToString(), UserRole.USER.ToString()})
        .Resolve(ctx => ctx.Service<IEmployeeModifiers>().DeleteEmployee(ctx.ArgumentValue<int>("id")));
    }

  }
}