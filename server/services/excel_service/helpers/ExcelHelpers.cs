using System;
using System.Collections.Generic;
using OfficeOpenXml;
using server.database.enums;
using server.services.excel_service.models;
using server.services.import_service.models;
using server.services.properties_service.models;

namespace server.services.excel_service.helpers
{
  public static class ExcelHelpers
  {
    /// <summary>
    /// Read data from file.
    /// </summary>
    public static List<EmployeeImport> ReadDataToImport(this ExcelWorksheet worksheet, EmployeeColumnProperties properties)
    {
      var startRow = 2;
      var endRow = worksheet.Dimension.End.Row;

      // Initialize empty data list
      var worksheetData = new List<List<CellValue>>();

      // Check if the file contains any records
      var totalRows = endRow - startRow;

      // Validate
      if (totalRows <= 1) { throw new Exception("The input file is empty."); }

      var employeesInput = new List<EmployeeImport>();

      // Iterate through table content
      for (int row = startRow; row <= endRow; row++)
      {
        var employeeId = worksheet.Cells[row, properties.EmployeeId.ColumnNumber];
        if (employeeId == null) { throw new Exception($"The employee \"ID\" must have a value [row: {row}]"); }

        var firstName = worksheet.Cells[row, properties.EmployeeId.ColumnNumber];
        if (employeeId == null) { throw new Exception($"The employee \"FirstName\" must have a value [row: {row}]"); }

        var lastName = worksheet.Cells[row, properties.EmployeeId.ColumnNumber];
        if (employeeId == null) { throw new Exception($"The employee \"LastName\" must have a value [row: {row}]"); }

        var discipline = worksheet.Cells[row, properties.EmployeeId.ColumnNumber];
        if (employeeId == null) { throw new Exception($"The employee \"Discipline\" must have a value [row: {row}]"); }

        var employee = new EmployeeImport
        {
          EmployeeId = employeeId.GetValue<int>(),
          FirstName = firstName.GetValue<string>().Trim(),
          LastName = lastName.GetValue<string>().Trim(),
          Discipline = discipline.GetValue<string>().Trim().ToEmployeeDisciplineEnum()
        };
        employeesInput.Add(employee);
      }
      return employeesInput;
    }


    /// <summary>
    /// Try to convert cell value to boolean.
    /// </summary>
    private static void ConvertValue(this ExcelRange cell)
    {
      var value = cell.Value?.ToString();
      if (value == null) { cell.Value = false; }
      else { cell.Value = true; }
    }

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