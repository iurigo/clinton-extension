using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using server.database.contexts;
using server.database.models;
using server.database.sources;
using server.database.enums;

namespace server.graphql.mutation.modifiers
{
  public interface ISystemSettingsModifiers
  {
    Task<bool> SetSystemSettings(string key, string value);
    Task<bool> ClearSystemSettings(string key);
  }


  public class SystemSettingsModifiers : ISystemSettingsModifiers
  {
    private readonly DataContext _db;
    private readonly ISystemLogModifiers _logs;


    public SystemSettingsModifiers(DataContext db, ISystemLogModifiers logs)
    {
      this._db = db;
      this._logs = logs;
    }


    /// <summary>
    /// Set system settings value
    /// </summary>
    public async Task<bool> SetSystemSettings(string key, string value)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing settings
      var settings = await this._db.SystemSettings.FirstOrDefaultAsync(s => s.Key == key);

      if (settings == null)
      {
        // Create a new record
        settings = new SystemSettings
        {
          Key = key,
          Value = value,
          CreatedAt = now,
          UpdatedAt = now
        };
        this._db.SystemSettings.Add(settings);
      }
      else
      {
        // Update an existing record
        settings.Value = value;
        settings.UpdatedAt = now;
        this._db.SystemSettings.Update(settings);
      }

      // Add system log record
      this._logs.Write(
        source: EventSource.SYSTEM,
        type: EventType.OTHER,
        details: new { key = key, value = value },
        date: now
      );

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }


    /// <summary>
    /// Clear system settings by key
    /// </summary>
    public async Task<bool> ClearSystemSettings(string key)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing settings
      var settings = await this._db.SystemSettings.FirstOrDefaultAsync(s => s.Key == key);

      if (settings == null)
      {
        return false;
      }
      else
      {
        settings.Value = null;
        settings.UpdatedAt = now;
        settings.DeletedAt = now;
        this._db.SystemSettings.Update(settings);
      }

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }

  }
}