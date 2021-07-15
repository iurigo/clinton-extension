using System;
using System.Collections.Generic;
using server.database.contexts;
using server.services.properties_service.models;

namespace server.services.properties_service
{
  public interface IPropertiesService
  {
    EmployeeColumnProperties GetEmployeeImportProperties();
  }
  public class PropertiesService : IPropertiesService
  {
    private DataContext _db;
    public PropertiesService(DataContext db)
    {
      this._db = db;
    }

    /// <summary>
    /// Get employee import properties
    /// </summary>
    public EmployeeColumnProperties GetEmployeeImportProperties()
    {
      return new EmployeeColumnProperties()
      {
        EmployeeId = new ColumnProperty { Name = "ID", Type = ColumnType.String, ColumnNumber = 2 },
        FirstName = new ColumnProperty { Name = "FirstName", Type = ColumnType.String, ColumnNumber = 4 },
        LastName = new ColumnProperty { Name = "LastName", Type = ColumnType.String, ColumnNumber = 6 },
        Discipline = new ColumnProperty { Name = "Discipline", Type = ColumnType.Enum, ColumnNumber = 27 }
      };
    }
  }
}