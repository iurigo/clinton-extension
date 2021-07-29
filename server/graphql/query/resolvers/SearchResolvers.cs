using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using server.graphql.types;
using server.database.models;
using server.graphql.models;
using server.database.contexts;
using server.graphql.extensions;

namespace server.graphql.query.resolvers
{
  public interface ISearchResolvers
  {
    Task<Pagination<Employee>> EmployeeSearch(KeyInfo info, string value);
  }


  public class SearchResolvers : ISearchResolvers
  {
    private readonly DataContext _db;

    public SearchResolvers(
      IServiceScopeFactory scopeFactory
    )
    {
      this._db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }


    /// <summary>
    /// The global employee search
    /// </summary>
    public async Task<Pagination<Employee>> EmployeeSearch(KeyInfo info, string value)
    {
      // Generate enhanced key info
      var keyInfo = new KeyInfo
      {
        Skip = info.Skip,
        Take = info.Take,
        Filter = info.Filter.ExtendRootFilter(value),
        Sort = info.Sort
      };

      // Get the data
      var employees = await this._db.Employees.AsNoTracking().ToPageableQuery(keyInfo);

      // Return the result
      return new Pagination<Employee> { Data = employees.Data, TotalCount = employees.TotalCount };;
    }
  }
}