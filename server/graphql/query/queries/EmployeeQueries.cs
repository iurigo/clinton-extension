using System.Threading;
using HotChocolate.Resolvers;
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
          descriptor.Field("employees")
           .Description("Get the list of employees.")
           .UsePagination<EmployeeType, Employee>()
           .Authorize()
           .Resolver(ctx => ctx.Service<IEmployeeResolvers>().GetEmployees(ctx.GetKeyInfo()));

          descriptor.Field("employee")
           .Description("Get the employee by id.")
           .Argument("id", a => a.Type<NonNullType<IntType>>().Description("The unique identifier."))
           .Type<EmployeeType>()
           .Authorize()
           .Resolver(ctx =>
        {
          return ctx.BatchDataLoader<int, Employee>(nameof(IEmployeeResolvers.GetEmployeeById),
            keys => ctx.Service<IEmployeeResolvers>().GetEmployeeById(keys))
            .LoadAsync(ctx.Argument<int>("id"), CancellationToken.None);
        }); 
        }
    }
}