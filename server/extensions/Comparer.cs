using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace server.extensions
{
  /// <summary>
  /// Compares two objects for the difference in property values.
  /// Use placeholders "[Name]", "[ValueA]" and "[ValueB]" in the constuctor to define output message format.
  /// </summary>
  public class ObjectComparer
  {
    private readonly string _message;

    public ObjectComparer()
    {
      this._message = "The \"[name]\" property value was changed from \"[valueA]\" to \"[valueB]\"";
    }

    public ObjectComparer(string message)
    {
      this._message = message;
    }

    public List<string> CompareObjects<T1, T2>(T1 objA, T2 objB)
    {
      // Get the list of properties/fields of the T1 class
      var propertyNames = new List<string>();
      foreach (var memberInfo in typeof(T1).GetMembers())
      {
        if (memberInfo.MemberType == MemberTypes.Property || memberInfo.MemberType == MemberTypes.Field)
        {
          propertyNames.Add(memberInfo.Name);
        }
      }

      // Compare
      return CompareObjects(objA, objB, propertyNames);
    }

    public List<string> CompareObjects<T1, T2>(T1 objA, T2 objB, IEnumerable<string> propertyNames)
    {
      // Validate input parameters
      var t1MemebersInfo = typeof(T1).GetMembers();
      var t2MembersInfo = typeof(T2).GetMembers();
      if (!propertyNames.All(p => t1MemebersInfo.Any(t1 => t1.Name == p) && t2MembersInfo.Any(t2 => t2.Name == p)))
      {
        throw new ArgumentException("Not all properties are present in the both objects.");
      }

      // Stores all property value differences
      var differences = new List<string>();

      // Compare each property and generate the list of differences
      foreach (var name in propertyNames)
      {
        var valueA = GetStringValue(GetPropertyOrFieldValue(objA, name));
        var valueB = GetStringValue(GetPropertyOrFieldValue(objB, name));
        if (valueA != valueB)
        {
          differences.Add(this._message.Replace("[name]", name).Replace("[valueA]", valueA).Replace("[valueB]", valueB));
        }
      }

      return differences;
    }

    public List<string> CompareValues<T>(T obj, IDictionary<string, object> properties)
    {
      // Validate input parameters
      if (properties == null || !properties.Keys.Any())
      {
        throw new ArgumentException(nameof(properties));
      }

      // Stores all property value differences
      var differences = new List<string>();

      // Compare each property and generate the list of differences
      foreach (var name in properties.Keys)
      {
        var valueA = GetStringValue(GetPropertyOrFieldValue(obj, name));
        var valueB = GetStringValue(properties[name]);
        if (valueA != valueB)
        {
          differences.Add(this._message.Replace("[name]", name).Replace("[valueA]", valueA).Replace("[valueB]", valueB));
        }
      }

      return differences;
    }

    private object GetPropertyOrFieldValue<T>(T obj, string propertyName)
    {
      var type = typeof(T);

      // Try to get property value
      var propertyInfo = type.GetProperty(propertyName);
      if (propertyInfo != null) { return propertyInfo.GetValue(obj); }

      // Try to get field value
      var fieldInfo = type.GetField(propertyName);
      if (propertyInfo != null) { return fieldInfo.GetValue(obj); }

      throw new ArgumentException(nameof(propertyName));
    }

    private string GetStringValue(object value)
    {
      if (value == null) { return null; }
      if (value is List<int>) { return string.Join(", ", (List<int>)value); }
      if (value is Decimal) { return string.Format("{0:N2}", value); }
      return value.ToString();
    }

  }
}