using System;

namespace server.settings
{
  public static class SettingsExtensions
  {
    public static void Validate()
    {
      var list = new string[]
      {
        "APP_VERSION",
        "APP_DB",
        "APP_JWT_KEY"
      };

      foreach (var item in list)
      {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(item)))
        {
          throw new ArgumentNullException($"[{item}] environment variable is not set.");
        }
      }
    }
  }
}