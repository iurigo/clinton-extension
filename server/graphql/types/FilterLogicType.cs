using HotChocolate.Types;

namespace server.graphql.types
{
  public enum FilterLogic
  {
    AND = 1,
    OR = 2
  }


  public sealed class FilterLogicType : EnumType<FilterLogic> { }
}