using HotChocolate.Types;
using server.graphql.query.queries;

namespace server.graphql.query
{
  public class Query : ObjectType
  {
    /// <summary>
    /// GraphQL query entry point
    /// </summary>
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Description("GraphQL query entry point.");
      
      AuthQueries.Register(descriptor);
      InfoQueries.Register(descriptor);
      UserQueries.Register(descriptor);
      EmployeeQueries.Register(descriptor);
      SystemSettingsQueries.Register(descriptor);
    }

  }
}