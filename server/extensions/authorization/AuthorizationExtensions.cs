using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using server.database.enums;

namespace server.extensions
{
  public static class AuthorizationExtensions
  {
    public static int? GetUserId(this HttpContext http)
    {
      return http?.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.NameIdentifier)?.Value.ConvertTo<int?>();
    }

    public static bool IsAdmin(this HttpContext http)
    {
      UserRole isAdmin;
      Enum.TryParse(http.User.Claims.Where((c) => c.Type == ClaimTypes.Role).Select(i => i.Value).FirstOrDefault(), out isAdmin);
      return isAdmin == UserRole.ADMIN;
    }
  }
}