using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using server.database.enums;
using server.database.models;
using server.extensions;
using server.graphql.mutation.types.employee;
using server.services.import_service.models;

namespace server.services.import_service.helpers
{
  public static class ImportHelpers
  {
    /// <summary>
    /// Convert employee
    /// </summary>
    public static EmployeeImport ToModel(this Employee employee)
    {
      return new EmployeeImport
      {
        Id = employee.Id,
        EmployeeId = employee.EmployeeId,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Discipline = employee.Discipline,
        Rate = employee.Rate,
        IsActive = employee.IsActive
      };
    }

    /// <summary>
    /// Convert employees
    /// </summary>
    public static List<EmployeeImport> ToModel(this List<Employee> employees)
    {
      return employees.Select(employee => employee.ToModel()).ToList();
    }

    /// <summary>
    /// Change "Active" status employees
    /// </summary>
    public static List<EmployeeImport> SetStatus(this List<EmployeeImport> employees, bool isActive)
    {
      // Change status and return
      return employees.Select(employee => { employee.IsActive = isActive; return employee; }).ToList();
    }

    /// <summary>
    /// Convert employee
    /// </summary>
    public static EmployeeInput ToModel(this EmployeeImport employee)
    {
      return new EmployeeInput
      {
        Id = employee.Id,
        EmployeeId = employee.EmployeeId,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Discipline = employee.Discipline,
        Rate = employee.Rate.Value,
        IsActive = employee.IsActive
      };
    }

    /// <summary>
    /// Convert employees
    /// </summary>
    public static List<EmployeeInput> ToModel(this List<EmployeeImport> employees)
    {
      return employees.Select(employee => employee.ToModel()).ToList();
    }

    /// <summary>
    /// Converts dataTable to employees
    /// </summary>
    public static List<EmployeeImport> GetEmployeesToImport(this DataTable dataTable)
    {
       // Result list
      var employeesToImport = new List<EmployeeImport>();

      // Read the data
      for (int row = 0; row < dataTable.Rows.Count; row++)
      {
        var employeeId = dataTable.Rows[row].Field<string>("ID");
        if (string.IsNullOrWhiteSpace(employeeId)) { throw new Exception($"The employee \"ID\" must have a value [row: {row + 1}]"); }

        var firstName = dataTable.Rows[row].Field<string>("FirstName");
        if (string.IsNullOrWhiteSpace(firstName)) { throw new Exception($"The employee \"FirstName\" must have a value [row: {row + 1}]"); }

        var lastName = dataTable.Rows[row].Field<string>("LastName");
        if (string.IsNullOrWhiteSpace(lastName)) { throw new Exception($"The employee \"LastName\" must have a value [row: {row + 1}]"); }

        var discipline = dataTable.Rows[row].Field<string>("Discipline");
        if (string.IsNullOrWhiteSpace(discipline)) { throw new Exception($"The employee \"Discipline\" must have a value [row: {row + 1}]"); }
        
        employeesToImport.Add(new EmployeeImport
        { 
          EmployeeId = employeeId.ConvertTo<int>(),
          FirstName = firstName,
          LastName = lastName,
          Discipline = discipline.ToEmployeeDisciplineEnum(),
        });
      }

      // Return the result
      return employeesToImport;
    }

    /// <summary>
    /// Converts string to enum
    /// </summary>
    public static EmployeeDiscipline ToEmployeeDisciplineEnum(this string employeeDiscipline)
    {
      switch (employeeDiscipline)
      {
        case "PA": return EmployeeDiscipline.PA;
        case "PCA": return EmployeeDiscipline.PCA;
        case "HHA": return EmployeeDiscipline.HHA;
        case "PCA | HHA": return EmployeeDiscipline.PCA_OR_HHA;
        default: throw new Exception("Unknown employee discipline.");
      }
    }
  }
}