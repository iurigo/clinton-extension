using System;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server.database.contexts;
using server.graphql.directives;
using server.graphql.mutation;
using server.graphql.mutation.modifiers;
using server.graphql.query;
using server.graphql.query.resolvers;
using server.middlewares;
using server.services.jwt_service;
using server.settings;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using server.services.properties_service;
using server.services.excel_service;
using server.services.import_service;
using server.services.csv;

namespace server
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // Get application settings
      SettingsExtensions.Validate();
      var db = Environment.GetEnvironmentVariable("APP_DB");
      var jwtKey = Environment.GetEnvironmentVariable("APP_JWT_KEY");

      // Authentication
      services.AddJwtAuthentication(jwtKey);
      services.AddAuthorization();

      // Services
      services.AddTransient<IPropertiesService, PropertiesService>();
      services.AddTransient<IExcelService, ExcelService>();
      services.AddTransient<ICsvService, CsvService>();
      services.AddTransient<IEmployeeImportService, EmployeeImportService>();
      services.AddSingleton<IJwtService>(new JwtService(jwtKey));

      // Database
      services.AddDbContextPool<DataContext>(o => o.UseSqlServer(db));

      // GraphQL
      services.AddGraphQL(sp => SchemaBuilder.New()
        .AddServices(sp)
        .AddAuthorizeDirectiveType()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddDirectiveType<FilterDirective>()
        .AddDirectiveType<SortDirective>()
        .Create()
      );
      services.AddDataLoaderRegistry();

       // GraphQL resolvers
      services.AddTransient<IAccessTokenResolvers, AccessTokenResolvers>();
      services.AddTransient<IInfoResolvers, InfoResolvers>();
      services.AddTransient<IUserResolvers, UserResolvers>();
      services.AddTransient<ISystemLogResolvers, SystemLogResolvers>();
      services.AddTransient<IEmployeeResolvers, EmployeeResolvers>();
      services.AddTransient<ISystemSettingsResolvers, SystemSettingsResolvers>();

      // GraphQL modifiers
      services.AddTransient<IUserModifiers, UserModifiers>();
      services.AddTransient<ISystemLogModifiers, SystemLogModifiers>();
      services.AddTransient<ISystemSettingsModifiers, SystemSettingsModifiers>();
      services.AddTransient<IEmployeeModifiers, EmployeeModifiers>();

      // API services
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinton Extension", Version = "v1" });
      });

      // CORS
      services.AddCors(o =>
      {
        o.AddDefaultPolicy(p =>
        {
          p.AllowAnyOrigin();
          p.AllowAnyHeader();
          p.AllowAnyMethod();
          p.WithExposedHeaders("Content-Disposition");
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // Development
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinton Extension v1"));
      }
      
      // System
      app.UseCors();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseErrorHandler();

      // GraphQL
      app.UseGraphQL("/graphql");

      // Rest
      app.UseEndpoints(o =>
      {
        o.MapControllers();
      });
    }

  }
}
