using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using server.extensions;
using server.formatters;

namespace server.graphql.extensions
{
  public static class ScalarTypeExtensions
  {
    public static object ValueNodeToObject(this IValueNode literal)
    {
      if (literal == null) { throw new ArgumentNullException(nameof(literal)); }

      if (literal is NullValueNode) { return null; }
      if (literal is StringValueNode s)
      {
        // DateTimeOffset
        var date = s.Value.ToDateTimeOffset();
        if (date.HasValue) { return date.Value; }

        // String
        return s.Value;
      }
      if (literal is IntValueNode i) { return int.Parse(i.Value); }
      if (literal is FloatValueNode f) { return double.Parse(f.Value); }
      if (literal is BooleanValueNode b) { return b.Value; }
      if (literal is ObjectValueNode o) { return o.Fields.ToDictionary(k => k.Name.Value, v => v.Value.ValueNodeToObject()); }
      if (literal is ListValueNode l) { return l.Items.Select(x => x.ValueNodeToObject()).ToList(); }

      throw new ArgumentException(nameof(literal));
    }

    public static IValueNode ObjectToValueNode(this object value)
    {
      if (value == null) { return new NullValueNode(null); }
      if (value is string s) { new StringValueNode(value.ToString()); }
      if (value is DateTimeOffset d) { new StringValueNode((string)d.Serialize()); }
      if (value is int i) { return new IntValueNode(i); }
      if (value is float f) { return new FloatValueNode(f); }
      if (value is bool b) { return new BooleanValueNode(b); }
      if (value is IEnumerable<object> l) { return new ListValueNode(l.Select(x => x.ObjectToValueNode()).ToList()); }

      throw new ArgumentException(nameof(value));
    }

    public static object Serialize(this object value)
    {
      if (value == null) { return null; }
      if (value is string || value is int || value is float || value is decimal || value is bool) { return value; }
      if (value is DateTimeOffset d) { return d.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
      if (value is IEnumerable<object> lo) { return lo.Select(i => i.Serialize()).ToList(); }
      if (value is IEnumerable<int> li) { return li.Select(i => i.Serialize()).ToList(); }
      if (value is IEnumerable<float> lf) { return lf.Select(i => i.Serialize()).ToList(); }
      if (value is Dictionary<string, object> o)
      {
        return o.ToDictionary(k => k.Key.ToLowerFirstChar(), v => v.Value.Serialize());
      }

      return value.GetType().GetProperties()
        .ToDictionary(k => k.Name.ToLowerFirstChar(), v => v.GetValue(value).Serialize())
        .Where(w => w.Value != null)
        .ToDictionary(k => k.Key, v => v.Value);
    }

  }
}