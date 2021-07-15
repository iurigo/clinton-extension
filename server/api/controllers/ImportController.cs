using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using server.services.csv;
using server.services.excel_service;
using server.services.import_service;
using server.services.import_service.models;

namespace server.controllers
{
  [Authorize(Roles = "ADMIN")]
  [Route("api/import")]
  public class ImportController : Controller
  {
    private IExcelService _excelService;
    private ICsvService _csvService;
    private IEmployeeImportService _importService;
    public ImportController(IExcelService excelService, IEmployeeImportService importService, ICsvService csvService)
    {
      this._excelService = excelService;
      this._csvService = csvService;
      this._importService = importService;
    }

    /// <summary>Import data from excel file into database. </summary>
    /// <response code="200">Case 1: Import with error. </response>
    /// <response code="204">Case 2: Import successful. </response>
    /// <param name="file">Excel file to import data from.</param>
    [HttpPost]
    public async Task<EmployeeDataToImport> ImportDataFromExcel([FromHeader] IFormFile file)
    {
      // Read the file to import.
      if (file == null) { throw new Exception("File was not selected."); }
     
      var reader = new BinaryReader(file.OpenReadStream());
      
      DataTable dataTable;
      using (var stream = new MemoryStream())
      {
        // Read the file
        await file.CopyToAsync(stream);
        dataTable = this._csvService.Read(stream.ToArray());
      }
      
      // Generate sorted list with the employees
      var employeesToImport = await this._importService.SortEmployeeImportData(dataTable);

      // employeesToImport.EmployeesToAdd = employeesToImport.EmployeesToAdd.Select(e => { e.Rate = 12; return e; }).ToList();
      // employeesToImport.EmployeesToUpdate = employeesToImport.EmployeesToUpdate.Select(e => { e.Rate = 12; return e; }).ToList();

      // await this._importService.ImportEmployeeData(employeesToImport);
    
      // Return the data
      return employeesToImport;
    }
  }
}