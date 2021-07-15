using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.database.contexts;
using server.database.models;
using server.graphql.models;
using server.graphql.extensions;

namespace server.graphql.query.resolvers
{
  public interface ISystemLogResolvers
  {
    Task<ILookup<Key, SystemLog>> GetSystemLogByUserId(IReadOnlyCollection<Key> keys);
  }


  public class SystemLogResolvers : ISystemLogResolvers
  {
    private readonly DataContext _db;

    public SystemLogResolvers(IServiceScopeFactory scopeFactory)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }


    /// <summary>
    /// Get system logs by user ids
    /// </summary>
    public async Task<ILookup<Key, SystemLog>> GetSystemLogByUserId(IReadOnlyCollection<Key> keys)
    {
      // Process each individual keys separately
      var result = new List<KeyValuePair<Key, SystemLog>>();
      foreach (var key in keys)
      {
        // Get the data
        var data = await this._db.SystemLogs.Where(u => u.UserId == key.Id).ToQuery(key);
        result.AddRange(data.Select(i => new KeyValuePair<Key, SystemLog>(key, i)));
      }
      return result.ToLookup(k => k.Key, v => v.Value);
    }

  }
}