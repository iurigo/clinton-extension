using System;
using server.database.enums;

namespace server.database.models
{
  public class SystemLog
  {
    // Key
    public long Id { get; set; }

    // Properties
    public string Source { get; set; }
    public EventType Type { get; set; }

    /// <summary>System log details in JSON format</summary>
    public string Details { get; set; }
    public DateTimeOffset Date { get; set; }

    // Parents
    public int? UserId { get; set; }
    public User User { get; set; }
  }
}