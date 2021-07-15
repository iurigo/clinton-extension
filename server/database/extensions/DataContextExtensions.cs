using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using server.database.models;

namespace server.database.extensions
{
  public static class DataContextExtensions
  {
    /// <summary>
    /// Tries to connect to database and ensures that the latest migrations were applied
    /// </summary>
    /// <param name="db">DataContext reference</param>
    /// <param name="retries">The number of database connection retries</param>
    /// <param name="timeout">Timeout in seconds, before trying to reconnect to database</param>
    public static async Task Connect(this DbContext db, ILogger logger, int retries = 10, int timeout = 10)
    {
      for (int i = 0; i < retries; i++)
      {
        try
        {
          // Apply the latest migrations automatically
          await db.Database.MigrateAsync();
          return;
        }
        catch (Exception ex)
        {
          logger.LogError($"Could not connect to database. {ex.Message}. Retry ({i + 1}/{retries}) in {timeout} seconds.");
        }
        await Task.Delay(timeout * 1000);
      }
      throw new Exception("Could not connect to database.");
    }
  }


}