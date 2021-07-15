using dotenv.net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using server.database.contexts;
using server.database.extensions;

namespace server
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // Build
      var app = CreateHostBuilder(args).Build();

      // Configuration
      using (var scope = app.Services.CreateScope())
      {
        // Logger
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Connect to Database
        logger.LogInformation("Connecting to database...");
        scope.ServiceProvider.GetRequiredService<DataContext>().Connect(logger).Wait();
      }

      // Run
      app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      // Load environment variables
      DotEnv.Config(throwOnError: false);

      // Build the application
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
    }

  }
}
