using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace server.formatters
{
  public static class ObjectFormatter
  {
    public static dynamic IsNotNullOrEmpty<T>(this T anyObject)
    {
      var t = anyObject.GetType();
      var returnClass = new ExpandoObject() as IDictionary<string, object>;
      foreach (var pr in t.GetProperties())
      {
        var val = pr.GetValue(anyObject);
        if (val is string && string.IsNullOrWhiteSpace(val.ToString())) { }
        else if (val == null) { }
        else { returnClass.Add(pr.Name.ToLowerFirstChar(), val); }
      }
      return returnClass;
    }

    public static T DeepClone<T>(this object item)
    {
      if (item == null) { return default(T); }
      return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(item, Formatting.Indented, new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      }));
    }
  }
}
