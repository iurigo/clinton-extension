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
  public interface IEmployeeResolvers
  {
    Task<Pagination<Employee>> GetEmployees(KeyInfo info);
    Task<IReadOnlyDictionary<int, Employee>> GetEmployeeById(IReadOnlyCollection<int> keys);
  }


  public class EmployeeResolvers : IEmployeeResolvers
  {
    private readonly DataContext _db;
    private readonly ISystemSettingsResolvers _systemSettings;

    public EmployeeResolvers(IServiceScopeFactory scopeFactory, ISystemSettingsResolvers systemSettings)
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
      this._systemSettings = systemSettings;
    }


    /// <summary>
    /// Get all employees
    /// </summary>
    public async Task<Pagination<Employee>> GetEmployees(KeyInfo info)
    {
      // Filter employees
      var employees = await this._db.Employees.AsNoTracking().ToPageableQuery(info);
      
      // Return the result
      return new Pagination<Employee> { Data = employees.Data, TotalCount = employees.TotalCount };
    }


    /// <summary>
    /// Get the employees by ids
    /// </summary>
    public async Task<IReadOnlyDictionary<int, Employee>> GetEmployeeById(IReadOnlyCollection<int> keys)
    {
      // Get the data
      var employees = await this._db.Employees.AsNoTracking().Where(u => keys.Any(k => k == u.Id)).ToListAsync();

      // Convert the list to s dictionary and return
      return employees.ToDictionary(c => c.Id);
    }
  }
}