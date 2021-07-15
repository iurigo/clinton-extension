using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using server.database.enums;
using server.database.contexts;
using server.database.models;
using server.extensions;

namespace server.graphql.mutation.modifiers
{
  public interface ISystemLogModifiers
  {
    void Write(string source, EventType type, dynamic details, DateTimeOffset date, int? userId = null);
  }


  public class SystemLogModifiers : ISystemLogModifiers
  {
    private readonly DataContext _db;
    private readonly IHttpContextAccessor _http;

    public SystemLogModifiers(DataContext db, IHttpContextAccessor http)
    {
      this._db = db;
      this._http = http;
    }


    /// <summary>
    /// Writes the system log to database
    /// </summary>
    public void Write(string source, EventType type, dynamic details, DateTimeOffset date, int? userId = null)
    {
      // Create a new log record
      var newLog = new SystemLog
      {
        Source = source,
        Type = type,
        UserId = userId ?? this._http.HttpContext.GetUserId(),
        Details = details != null ? JsonSerializer.Serialize(details) : null,
        Date = date
      };

      // Add the log record
      this._db.SystemLogs.Add(newLog);
    }

  }
}