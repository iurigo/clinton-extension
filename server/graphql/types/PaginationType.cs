using System.Collections.Generic;
using HotChocolate.Types;

namespace server.graphql.types
{
  public class Pagination<T> where T : class
  {
    public IEnumerable<T> Data { get; set; }
    public int TotalCount { get; set; }
  }


  public sealed class PaginationType<TType, T> : ObjectType<Pagination<T>> where TType : ObjectType where T : class
  {
    protected override void Configure(IObjectTypeDescriptor<Pagination<T>> descriptor)
    {
      descriptor.Description("Generic pagination type.");

      // Properties
      descriptor.Field(f => f.Data)
        .Name("data")
        .Description("The list of generic items.")
        .Type<NonNullType<ListType<NonNullType<TType>>>>();

      descriptor.Field(f => f.TotalCount)
        .Name("totalCount")
        .Description("Total amount of items.")
        .Type<NonNullType<IntType>>();
    }

  }
}