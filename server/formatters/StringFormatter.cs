namespace server.formatters
{
  public static class StringFormatter
  {
    public static string ToUpperFirstChar(this string text)
    {
      if (text == null) { return null; }
      if (text == string.Empty) { return string.Empty; }
      return char.ToUpper(text[0]) + text.Substring(1);
    }

    public static string ToLowerFirstChar(this string text)
    {
      if (text == null) { return null; }
      if (text == string.Empty) { return string.Empty; }
      return char.ToLower(text[0]) + text.Substring(1);
    }

  }
}
