using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using server.database.contexts;

namespace server.graphql.query.resolvers
{
  public interface ISystemSettingsResolvers
  {
    Task<string> GetSystemSettings(string key);
  }


  public class SystemSettingsResolvers : ISystemSettingsResolvers
  {
    private readonly DataContext _db;

    public SystemSettingsResolvers(IServiceScopeFactory scopeFactory)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }


    /// <summary>
    /// Get the system-settings by key
    /// </summary>
    public async Task<string> GetSystemSettings(string key)
    {
      // Get system settings by key
      return await _db.SystemSettings
        .Where(s => s.Key == key)
        .Select(s => s.Value)
        .FirstOrDefaultAsync();
    }

  }
}