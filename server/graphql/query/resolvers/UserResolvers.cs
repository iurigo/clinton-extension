using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server.database.contexts;
using server.database.models;
using server.graphql.extensions;
using server.graphql.models;
using server.graphql.types;

namespace server.graphql.query.resolvers
{
  public interface IUserResolvers
  {
    Task<List<User>> GetUsers(KeyInfo info);
    Task<IReadOnlyDictionary<int, User>> GetUserById(IReadOnlyCollection<int> keys);
  }


  public class UserResolvers : IUserResolvers
  {
    private readonly DataContext _db;

    public UserResolvers(IServiceScopeFactory scopeFactory)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }

    /// <summary>
    /// Get all users
    /// </summary>
    public async Task<List<User>> GetUsers(KeyInfo info)
    {
      var users = await this._db.Users.AsNoTracking().ToQuery(info);
      return users;
    }


    /// <summary>
    /// Get the users by ids
    /// </summary>
    public async Task<IReadOnlyDictionary<int, User>> GetUserById(IReadOnlyCollection<int> keys)
    {
      // Get the data
      var users = await this._db.Users.Where(u => keys.Any(k => k == u.Id)).AsNoTracking().ToListAsync();

      // Convert the list to a dictionary and return
      return users.ToDictionary(c => c.Id);
    }
  }
}