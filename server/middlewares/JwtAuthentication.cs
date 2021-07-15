using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace server.middlewares
{
  public static class JwtAuthentication
  {
    /// <summary>
    /// Add JWT authentication module
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="key">The access token validation key</param>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string key)
    {
      // Validate arguments
      if (services == null) { throw new ArgumentNullException(nameof(services)); }
      if (string.IsNullOrEmpty(key)) { throw new ArgumentException(nameof(key)); }

      // Access token validation parameters
      var tokenValidationParameters = new TokenValidationParameters
      {
        // Validate key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        // Validate token expiration
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false
      };

      // Add base authentication module
      services.AddAuthentication(o =>
      {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(o =>
      {
        o.TokenValidationParameters = tokenValidationParameters;
      });

      return services;
    }

  }
}