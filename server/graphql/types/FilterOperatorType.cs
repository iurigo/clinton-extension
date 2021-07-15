using HotChocolate.Types;

namespace server.graphql.types
{
  public enum FilterOperator
  {
    EQUAL = 1,
    NOT_EQUAL = 2,
    IS_NULL = 3,
    IS_NOT_NULL = 4,
    LESS_THAN = 5,
    LESS_THAN_OR_EQUAL = 6,
    GREATER_THAN = 7,
    GREATER_THAN_OR_EQUAL = 8,
    STARTS_WITH = 9,
    ENDS_WITH = 10,
    CONTAINS = 11,
    DOES_NOT_CONTAIN = 12,
    IS_EMPTY = 13,
    IS_NOT_EMPTY = 14
  }


  public sealed class FilterOperatorType : EnumType<FilterOperator> { }
}