using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using server.database.contexts;
using server.services.excel_service.helpers;
using server.services.import_service.models;
using server.services.properties_service;

namespace server.services.excel_service
{
  public interface IExcelService
  {
    List<EmployeeImport> ReadExcelFile(ExcelPackage package);
  }
  public class ExcelService : IExcelService
  {
    private readonly DataContext _db;
    private readonly IPropertiesService _propertiesService;
    public ExcelService(
      DataContext db,
      IPropertiesService propertiesService
    )
    {
      this._db = db;
      this._propertiesService = propertiesService;
    }


    /// <summary>
    /// Read excel file from external source. 
    /// </summary>
    public List<EmployeeImport> ReadExcelFile(ExcelPackage package)
    {
      // Read table name.
      var tableName = package.Workbook.Worksheets[1].Cells[1, 2].Value.ToString();

      // Get list of file worksheet properties
      var importProperties = this._propertiesService.GetEmployeeImportProperties();

      // Get worksheet
      var worksheet = package.Workbook.Worksheets.FirstOrDefault();

      // Get worksheet data
      var employeesToImport = worksheet.ReadDataToImport(importProperties);

      // Return result
      return employeesToImport;
    }
  }
}