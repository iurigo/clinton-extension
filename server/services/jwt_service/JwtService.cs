using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.database.enums;
using server.services.jwt_service.models;

namespace server.services.jwt_service
{
  public interface IJwtService
  {
    AccessToken GenerateAccessToken(string userId, UserRole role);
  }


  public class JwtService : IJwtService
  {
    public readonly string Key;


    public JwtService(string key)
    {
      // Validate arguments
      if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException(nameof(key)); }

      // Set Key
      this.Key = key;
    }


    public AccessToken GenerateAccessToken(string userId, UserRole role)
    {
      var now = DateTime.UtcNow;
      var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Key));
      var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, role.ToString())
      };

      var jwt = new JwtSecurityToken
      (
        claims: claims,
        expires: now.Add(TimeSpan.FromHours(6)),
        signingCredentials: signingCredentials
      );

      return new AccessToken
      {
        Token = new JwtSecurityTokenHandler().WriteToken(jwt),
        RefreshToken = Guid.NewGuid().ToString()
      };
    }

  }
}