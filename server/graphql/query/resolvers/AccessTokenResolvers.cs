using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using server.database.contexts;
using server.database.models;
using server.extensions;
using server.database.enums;
using server.services.jwt_service.models;
using server.services.jwt_service;

namespace server.graphql.query.resolvers
{
  public interface IAccessTokenResolvers
  {
    Task<AccessToken> GetByUsernameAndPassword(string username, string password);
    Task<AccessToken> GetAccessTokenByRefreshToken(string refreshToken);
  }


  public class AccessTokenResolvers : IAccessTokenResolvers
  {
    private readonly DataContext _db;
    private readonly IJwtService _jwt;


    public AccessTokenResolvers(IServiceScopeFactory scopeFactory, IJwtService jwt)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
      this._jwt = jwt;
    }


    /// <summary>
    /// Get access-token by username and password
    /// </summary>
    public async Task<AccessToken> GetByUsernameAndPassword(string username, string password)
    {
      using (var transaction = this._db.Database.BeginTransaction())
      {
        try
        {
          // Get the user
          var user = await this._db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
          
          // Validate user
          if (user == null) { throw new QueryException("Invalid username."); }
          if (!(user.Status == UserStatus.ACTIVE)) { throw new QueryException("The account was blocked."); }
          
          // Validate password
          if (!Converters.ValidateHash(password, user.PasswordHash, user.PasswordSalt)) { throw new QueryException("Invalid password."); }

          // Generate access-token
          var accessToken = await this.CreateAccessToken(user);

          // Commit the transition
          transaction.Commit();

          // Return the result
          return accessToken;
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }


    /// <summary>
    /// Get access-token by refresh-token
    /// </summary>
    public async Task<AccessToken> GetAccessTokenByRefreshToken(string refreshToken)
    {
      // Get the token
      var token = await this._db.RefreshTokens
        .AsNoTracking()
        .Include(i => i.User)
        .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

      if (token == null) { throw new QueryException("Invalid refresh-token."); }
      if (!(token.User.Status == UserStatus.ACTIVE)) { throw new QueryException("The account was blocked."); }
      if (token.Date.AddHours(6) < DateTimeOffset.Now) { throw new QueryException("The refresh-token expired."); }

      // Generate access-token
      var accessToken = await this.CreateAccessToken(token.User);

      // Return the result
      return accessToken;
    }


    #region [ INTERNAL ]
    private async Task<AccessToken> CreateAccessToken(User user)
    {
      // Create new access-token
      var accessToken = this._jwt.GenerateAccessToken(user.Id.ToString(), user.Role);

      // Add new refresh-token
      var newRefreshToken = new RefreshToken
      {
        Token = accessToken.RefreshToken,
        UserId = user.Id,
        Date = DateTimeOffset.Now
      };
      this._db.RefreshTokens.Add(newRefreshToken);

      // Save changes
      await this._db.SaveChangesAsync();

      // Return result
      return accessToken;
    }
    #endregion
  }
}