using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.database.contexts;
using server.graphql.mutation.modifiers;
using server.services.import_service.helpers;
using server.services.import_service.models;

namespace server.services.import_service
{
  public interface IEmployeeImportService
  {
    Task<EmployeeDataToImport> SortEmployeeImportData(DataTable dataTable);
    Task<bool> ImportEmployeeData(EmployeeDataToImport input);
  }
  
  public class EmployeeImportService : IEmployeeImportService
  {
    private readonly DataContext _db;
    private readonly ISystemLogModifiers _logs;
    private readonly IEmployeeModifiers _eployees;
    public EmployeeImportService(DataContext db, ISystemLogModifiers logs, IEmployeeModifiers eployees)
    {
      this._db = db;
      this._logs = logs;
      this._eployees = eployees;
    }
    

    /// <summary>
    /// Sort employee to import list 
    /// </summary>
    public async Task<EmployeeDataToImport> SortEmployeeImportData(DataTable dataTable)
    {
      // Convert to employees to import models
      var importEmployees = dataTable.GetEmployeesToImport();
      
      var employeeDataToImport = new EmployeeDataToImport
      {
        EmployeesToAdd = new List<EmployeeImport>(),
        EmployeesToUpdateStatus = new List<EmployeeImport>(),
        EmployeesToUpdate = new List<EmployeeImport>()
      };

      // Get all employees from database
      var allEmployees = await this._db.Employees.AsNoTracking().ToListAsync();

      // Get old employee ids
      var oldEmployeeIds = allEmployees.Select(e => e.EmployeeId).ToList();

      var importEmployeeIds = importEmployees.Select(e => e.EmployeeId).ToList();

      // Check removed employees and change "IsActive" status
      var removed = oldEmployeeIds.Except(importEmployeeIds).ToList();
      if (removed.Any())
      {
        var employeesToRemove = allEmployees.Where(e => removed.Contains(e.EmployeeId)).ToList();
        employeeDataToImport.EmployeesToUpdateStatus.AddRange(employeesToRemove.ToModel().SetStatus(false));
      }

      // Check added employees
      var added = importEmployeeIds.Except(oldEmployeeIds).ToList();
      if (added.Any())
      {
        var employeesToAdd = importEmployees.Where(e => added.Contains(e.EmployeeId)).ToList();
        employeeDataToImport.EmployeesToAdd.AddRange(employeesToAdd.SetStatus(true));
      }
      
      // Check updated employees
      var updated = new List<EmployeeImport>();
      foreach (var employee in importEmployees)
      {
        if (allEmployees.Any(e => e.EmployeeId == employee.EmployeeId && (e.FirstName != employee.FirstName || e.LastName != employee.LastName || e.Discipline != employee.Discipline)))
        {
          var existingEmployee = allEmployees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
          employee.Id = existingEmployee.Id;
          employee.IsActive = existingEmployee.IsActive;
          employee.Rate = existingEmployee.Rate;
          updated.Add(employee);
        }
      }
      employeeDataToImport.EmployeesToUpdate.AddRange(updated);

      // Return the result
      return employeeDataToImport;
    }


    /// <summary>
    /// Sort employee to import list 
    /// </summary>
    public async Task<bool> ImportEmployeeData(EmployeeDataToImport input)
    {
      // Update removed employees "IsActive" status
      await this._eployees.UpdateMultipleEmployeesStatus(input.EmployeesToUpdateStatus.ToModel());

      // Update employees
      await this._eployees.UpdateMultipleEmployees(input.EmployeesToUpdate.ToModel());

      // Add employees
      await this._eployees.CreateMultipleEmployees(input.EmployeesToAdd.ToModel());
      return true;
    }
  }
}