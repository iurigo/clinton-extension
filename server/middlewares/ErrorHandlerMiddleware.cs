using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace server.middlewares
{
  /// <summary>
  /// Middleware intercepts all unhandled exceptions and generates json response with an error message
  /// </summary>
  public static class ErrorHandlerMiddleware
  {
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
      app.Use(async (context, next) =>
      {
        try
        {
          await next();
        }
        catch (Exception ex)
        {
          await SendResponse(context, ex.Message);
          return;
        }
      });
      return app;
    }

    private static async Task SendResponse(HttpContext context, string message)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = 400;
      await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = message }));
    }

  }
}