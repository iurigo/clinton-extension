using System.Threading;
using HotChocolate.Types;
using server.database.models;
using server.graphql.query.resolvers;
using server.graphql.extensions;
using server.graphql.query.types.employee;

namespace server.graphql.query.queries
{
  public static class EmployeeQueries
  {
    public static void Register(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field("employeesPageable")
       .Description("Get the list of pageable employees.")
       .UsePagination<EmployeeType, Employee>()
       .Authorize()
       .Resolve(ctx => ctx.Service<IEmployeeResolvers>().GetPageableEmployees(ctx.GetKeyInfo()));

      descriptor.Field("employees")
        .Description("The list of employees.")
        .Type<ListType<EmployeeType>>()
        .Authorize()
        .Resolve(ctx => ctx.Service<IEmployeeResolvers>().GetEmployees(ctx.GetKeyInfo()));

      descriptor.Field("employee")
      .Description("Get the employee by id.")
      .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The unique identifier."))
      .Type<EmployeeType>()
      .Authorize()
      .Resolve(ctx =>
      {
        return ctx.BatchDataLoader<int, Employee>(
          (keys, _) => ctx.Service<IEmployeeResolvers>().GetEmployeeById(keys),
          nameof(IEmployeeResolvers.GetEmployeeById)
        ).LoadAsync(ctx.ArgumentValue<int>("id"), CancellationToken.None);
      });

      descriptor.Field("searchInEmployees")
        .Description("Global search in all employees.")
        .Argument("value", a => a.Type<NonNullType<StringType>>().Description("The search value."))
        .UsePagination<EmployeeType, Employee>()
        .Authorize()
        .Resolve(ctx => ctx.Service<ISearchResolvers>().EmployeeSearch(ctx.GetKeyInfo(), ctx.ArgumentValue<string>("value")));
    }
  }
}