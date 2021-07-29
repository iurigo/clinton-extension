using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.graphql.mutation.modifiers;
using server.services.csv;
using server.services.excel_service;
using server.services.import_service;
using server.services.import_service.helpers;
using server.services.import_service.models;

namespace server.controllers
{
  [Authorize(Roles = "ADMIN")]
  [Route("api/import/employees")]
  public class ImportController : ControllerBase
  {
    private IExcelService _excelService;
    private ICsvService _csvService;
    private IEmployeeImportService _importService;
    private IEmployeeModifiers _employees;
    public ImportController(
      IExcelService excelService,
      IEmployeeImportService importService,
      ICsvService csvService,
      IEmployeeModifiers employees
    )
    {
      this._excelService = excelService;
      this._csvService = csvService;
      this._importService = importService;
      this._employees = employees;
    }

    /// <summary>Import data from excel file into database. </summary>
    /// <response code="200">Case 1: Import with error. </response>
    /// <response code="204">Case 2: Import successful. </response>
    /// <param name="file">Excel file to import data from.</param>
    [HttpPost]
    public async Task<ActionResult> ImportDataFromExcel([FromForm] IFormFile file)
    {
      // Read the file to import.
      if (file == null) { throw new Exception("No file was provided."); }
           
      DataTable dataTable;
      using (var stream = new MemoryStream())
      {
        // Read the file
        await file.CopyToAsync(stream);
        dataTable = this._csvService.Read(stream.ToArray());
      }
      
      // Generate sorted list with the employees
      var employeesToImport = await this._importService.SortEmployeeImportData(dataTable);

      // Update employees
      if (employeesToImport.EmployeesToUpdate.Any())
      {
        await this._employees.UpdateMultipleEmployees(employeesToImport.EmployeesToUpdate.ToModel());
      }

      // Change employees status
      if (employeesToImport.EmployeesToUpdateStatus.Any())
      {
        await this._employees.UpdateMultipleEmployeesStatus(employeesToImport.EmployeesToUpdateStatus.ToModel());
      }

      // await this._importService.ImportEmployeeData(employeesToImport);
    
      // Return the data
      return Ok(employeesToImport.EmployeesToAdd);
    }
  }
}