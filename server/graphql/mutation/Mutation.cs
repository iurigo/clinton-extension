using HotChocolate.Types;
using server.graphql.mutation.mutations;

namespace server.graphql.mutation
{
  /// <summary>
  /// GraphQL mutation entry point
  /// </summary>
  public class Mutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Description("GraphQL mutation entry point.");

      // AuthMutations.Register(descriptor);
      UserMutations.Register(descriptor);
      EmployeeMutations.Register(descriptor);
      SystemSettingsMutations.Register(descriptor);
    }
  }
}