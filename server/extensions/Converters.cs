using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using HotChocolate.Execution;

namespace server.extensions
{
  public static class Converters
  {
    public static T ConvertTo<T>(this object value)
    {
      var t = typeof(T);

      if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
      {
        if (value == null)
        {
          return default(T);
        }

        t = Nullable.GetUnderlyingType(t);
      }
      // if (!(value is IConvertible)) 
      return (T)Convert.ChangeType(value, t);
    }

    public static (string hash, string salt) Hash(string value)
    {
      var salt = BCrypt.Net.BCrypt.GenerateSalt();
      var hash = BCrypt.Net.BCrypt.HashPassword(value, salt);

      return (hash, salt);
    }

    /// <summary>
    /// Generate SHA256 hash of the bytes array
    /// </summary>
    /// <param name="value">Bytes array</param>
    public static string Hash(byte[] value)
    {
      // Validate argument
      if (value == null || value.Length == 0) { return null; }
      
      // Calculate and return the hash of the value
      var hashBytes = SHA256.Create().ComputeHash(value);
      return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static bool ValidateHash(string password, string hash, string salt)
    {
      return hash == BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public static DateTimeOffset? ToDateTimeOffset(this string input)
    {
      // Validate input string
      if (input == null || input.Length != 24) { return null; }

      // Try to parse the date
      DateTimeOffset date;
      if (DateTimeOffset.TryParse(input, out date)) { return date.ToLocalTime(); }

      // Return null if date was not parsed
      return null;
    }
  }

  /// <summary>
  /// Use this converter ONLY for DESERIALIZE 'object' type
  /// </summary>
  public class ObjectToInferredTypesConverter
    : JsonConverter<object>
  {
    public override object Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      if (reader.TokenType == JsonTokenType.True)
      {
        return true;
      }

      if (reader.TokenType == JsonTokenType.False)
      {
        return false;
      }

      if (reader.TokenType == JsonTokenType.Number)
      {
        if (reader.TryGetInt64(out long l))
        {
          return l;
        }

        return reader.GetDouble();
      }

      if (reader.TokenType == JsonTokenType.String)
      {
        if (reader.TryGetDateTime(out DateTime datetime))
        {
          return datetime;
        }

        return reader.GetString();
      }

      if (reader.TokenType == JsonTokenType.Null)
      {
        return null;
      }

      // Use JsonElement as fallback.
      // Newtonsoft uses JArray or JObject.
      using JsonDocument document = JsonDocument.ParseValue(ref reader);
      return document.RootElement.Clone();
    }

    public override void Write(
      Utf8JsonWriter writer,
      object objectToWrite,
      JsonSerializerOptions options)
    {
      writer.WriteStringValue(JsonSerializer.Serialize(objectToWrite));
    }
  }
}