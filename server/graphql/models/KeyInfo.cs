using System.Collections.Generic;
using server.graphql.types;

namespace server.graphql.models
{
  public class KeyInfo
  {
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public FilterRoot Filter { get; set; }
    public List<Sort> Sort { get; set; }
  }
}