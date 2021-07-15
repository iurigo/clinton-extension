using System;
using System.Linq;
using System.Threading.Tasks;
using server.database.contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server.extensions;
using server.database.enums;

namespace server.graphql.query.resolvers
{
  public interface IInfoResolvers
  {
    string GetVersion();
    int GetCurrentUserId();
    Task<string> GetCurrentUserFullName();
    bool IsAdmin();
    Task<bool> IsActive();
  }


  public class InfoResolvers : IInfoResolvers
  {
    private readonly DataContext _db;
    private readonly IHttpContextAccessor _http;
    private readonly string _version;


    public InfoResolvers(IServiceScopeFactory scopeFactory, IHttpContextAccessor http)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
      this._http = http;
      this._version = Environment.GetEnvironmentVariable("APP_VERSION");
    }


    /// <summary>
    /// Get application version
    /// </summary>
    public string GetVersion()
    {
      return this._version;
    }


    /// <summary>
    /// Get current user's id
    /// </summary>
    public int GetCurrentUserId()
    {
      // Get current user id 
      return _http.HttpContext.GetUserId() ?? new int();
    }


    /// <summary>
    /// Get current user's full name
    /// </summary>
    public async Task<string> GetCurrentUserFullName()
    {
      // Get current user id 
      var userId = _http.HttpContext.GetUserId();
      if (userId == null) { return null; }

      // Get the full name
      var user = await _db.Users.Where(u => u.Id == userId.Value).FirstOrDefaultAsync();
      return user.FullName;
    }


    /// <summary>
    /// Check if current user is admin
    /// </summary>
    public bool IsAdmin()
    {
      return _http.HttpContext.IsAdmin();
    }


    /// <summary>
    /// Get current user's active state
    /// </summary>
    public async Task<bool> IsActive()
    {
      // Get current user id 
      var userId = _http.HttpContext.GetUserId();
      if (userId == null) { return false; }

      // Get the user
      var status = await _db.Users
        .Where(u => u.Id == userId.Value)
        .Select(u => u.Status)
        .FirstOrDefaultAsync();
      // Return the result
      return status == UserStatus.ACTIVE;
    }

  }
}