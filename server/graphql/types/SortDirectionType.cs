using HotChocolate.Types;

namespace server.graphql.types
{
  public enum SortDirection
  {
    ASC = 1,
    DESC = 2
  }


  public sealed class SortDirectionType : EnumType<SortDirection> { }
}