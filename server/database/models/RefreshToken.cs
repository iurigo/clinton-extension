using System;

namespace server.database.models
{
  public class RefreshToken
  {
    // Key
    public int UserId { get; set; }
    public string Token { get; set; }

    // Properties
    public DateTimeOffset Date { get; set; }

    // Parents
    public User User { get; set; }
  }
}
