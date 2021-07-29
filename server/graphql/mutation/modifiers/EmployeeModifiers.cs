using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using server.database.models;
using server.database.contexts;
using server.extensions;
using server.database.enums;
using server.graphql.mutation.types.employee;
using server.database.sources;
using System.Collections.Generic;

namespace server.graphql.mutation.modifiers
{
  public interface IEmployeeModifiers
  {
    Task<bool> CreateEmployee(EmployeeInput input);
    Task<bool> CreateMultipleEmployees(List<EmployeeInput> input);
    Task<bool> UpdateEmployee(EmployeeInput input);
    Task<bool> UpdateMultipleEmployees(List<EmployeeInput> input);
    Task<bool> UpdateMultipleEmployeesStatus(List<EmployeeInput> input);
    Task<bool> DeleteEmployee(int id);
    Task<bool> DeleteMultipleEmployees(List<int> ids);
  }


  public class EmployeeModifiers : IEmployeeModifiers
  {
    private readonly DataContext _db;
    private readonly ISystemLogModifiers _logs;

    public EmployeeModifiers(
      DataContext db,
      ISystemLogModifiers logs
    )
    {
      this._db = db;
      this._logs = logs;
    }


    /// <summary>
    /// Creates a new employee
    /// </summary>
    public async Task<bool> CreateEmployee(EmployeeInput input)
    {
      using (var transaction = this._db.Database.BeginTransaction())
      {
        try
        {
          // Get the current date
          var now = DateTimeOffset.Now;

          // Check for the duplicates
          var duplicate = await this._db.Employees.AnyAsync(i =>
            i.FirstName == input.FirstName &&
            i.LastName == input.LastName &&
            i.EmployeeId == input.EmployeeId
          );
          if (duplicate) { throw new QueryException("An employee already exists."); }

          // Create a new employee
          var employee = new Employee
          {
            EmployeeId = input.EmployeeId,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Discipline = input.Discipline,
            Rate = input.Rate,
            IsActive = input.IsActive,
            CreatedAt = now,
            UpdatedAt = now
          };

          // Add employee to database and save changes
          this._db.Employees.Add(employee);
          await this._db.SaveChangesAsync();

          // Add system log record
          this._logs.Write(source: EventSource.EMPLOYEE,
            type: EventType.CREATE,
            details: new
            {
              id = employee.Id,
              employeeId = employee.EmployeeId,
              firstName = input.FirstName,
              lastName = input.LastName,
              discipline = input.Discipline,
              rate = input.Rate,
              isActive = input.IsActive
            },
            date: now
          );

          // Save all changes
          await this._db.SaveChangesAsync();

          // Commit the transition
          transaction.Commit();

          // Return new employee
          return true;
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }


    /// <summary>
    /// Creates a new employee
    /// </summary>
    public async Task<bool> CreateMultipleEmployees(List<EmployeeInput> input)
    {
      using (var transaction = this._db.Database.BeginTransaction())
      {
        try
        {
          // Get the current date
          var now = DateTimeOffset.Now;

          // Create a new employees
          var newEmployees = input.Select(i => new Employee
          {
            EmployeeId = i.EmployeeId,
            FirstName = i.FirstName,
            LastName = i.LastName,
            Discipline = i.Discipline,
            Rate = i.Rate,
            IsActive = i.IsActive,
            CreatedAt = now,
            UpdatedAt = now
          }).ToList();

          // Add employee to database and save changes
          this._db.Employees.AddRange(newEmployees);
          await this._db.SaveChangesAsync();

          // Add system log records
          newEmployees.ForEach(employee =>
            {
              this._logs.Write(source: EventSource.EMPLOYEE,
                type: EventType.CREATE,
                details: new
                {
                  id = employee.Id,
                  employeeId = employee.EmployeeId,
                  firstName = employee.FirstName,
                  lastName = employee.LastName,
                  discipline = employee.Discipline,
                  rate = employee.Rate,
                  isActive = employee.IsActive
                },
                date: now
              );
            }
          );

          // Save all changes
          await this._db.SaveChangesAsync();

          // Commit the transition
          transaction.Commit();

          // Return new employee
          return true;
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }


    /// <summary>
    /// Updates the existing employee
    /// </summary>
    public async Task<bool> UpdateEmployee(EmployeeInput input)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing employee
      var existingEmployee = await this._db.Employees.FirstOrDefaultAsync(i => i.Id == input.Id);
      if (existingEmployee == null) { throw new QueryException("The employee was not found."); }

      // Compare existing model with the new one
      var changes = (new ObjectComparer()).CompareObjects(existingEmployee, input, new string[]
      {
        nameof(Employee.EmployeeId),
        nameof(Employee.FirstName),
        nameof(Employee.LastName),
        nameof(Employee.Discipline),
        nameof(Employee.Rate),
        nameof(Employee.IsActive)
      });

      // Write to employee log and save changes if there are any changes
      if (changes.Any())
      {
        // Set property values
        existingEmployee.EmployeeId = input.EmployeeId;
        existingEmployee.FirstName = input.FirstName;
        existingEmployee.LastName = input.LastName;
        existingEmployee.Discipline = input.Discipline;
        existingEmployee.Rate = input.Rate;
        existingEmployee.IsActive = input.IsActive;
        existingEmployee.UpdatedAt = now;

        // Write the log
        this._logs.Write(
          source: EventSource.EMPLOYEE,
          type: EventType.UPDATE,
          details: new
          {
            id = existingEmployee.Id,
            changes = changes
          },
          date: now
        );

        // Save all changes
        await this._db.SaveChangesAsync();
      }

      // Return success
      return true;
    }


    /// <summary>
    /// Updates the existing employees
    /// </summary>
    public async Task<bool> UpdateMultipleEmployees(List<EmployeeInput> input)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      foreach (var employee in input)
      {
        // Get an existing employee
        var existingEmployee = await this._db.Employees.FirstOrDefaultAsync(i => i.Id == employee.Id);
        if (existingEmployee == null) { throw new QueryException("The employee was not found."); }

        // Compare existing model with the new one
        var changes = (new ObjectComparer()).CompareObjects(existingEmployee, employee, new string[]
        {
          nameof(Employee.EmployeeId),
          nameof(Employee.FirstName),
          nameof(Employee.LastName),
          nameof(Employee.Discipline),
          nameof(Employee.Rate),
          nameof(Employee.IsActive)
        });

        // Write to employee log and save changes if there are any changes
        if (changes.Any())
        {
          // Set property values
          existingEmployee.EmployeeId = employee.EmployeeId;
          existingEmployee.FirstName = employee.FirstName;
          existingEmployee.LastName = employee.LastName;
          existingEmployee.Discipline = employee.Discipline;
          existingEmployee.Rate = employee.Rate;
          existingEmployee.IsActive = employee.IsActive;
          existingEmployee.UpdatedAt = now;

          // Update existing employee
          this._db.Update(existingEmployee);

          // Write the log
          this._logs.Write(
            source: EventSource.EMPLOYEE,
            type: EventType.UPDATE,
            details: new
            {
              id = existingEmployee.Id,
              changes = changes
            },
            date: now
          );
        }
      }

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }
    /// <summary>
    /// Updates status of the existing employees
    /// </summary>
    public async Task<bool> UpdateMultipleEmployeesStatus(List<EmployeeInput> input)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      foreach (var employee in input)
      {
        // Get an existing employee
        var existingEmployee = await this._db.Employees.FirstOrDefaultAsync(i => i.Id == employee.Id);
        if (existingEmployee == null) { throw new QueryException($"The employee with \"EmployeeId\": {employee.EmployeeId} was not found."); }

        // Set property values
        existingEmployee.IsActive = employee.IsActive;
        existingEmployee.UpdatedAt = now;

        // Update existing employee
        this._db.Update(existingEmployee);

        // Write the log
        this._logs.Write(
          source: EventSource.EMPLOYEE,
          type: EventType.UPDATE,
          details: new
          {
            id = existingEmployee.Id,
            isActive = existingEmployee.IsActive
          },
          date: now
        );
      }
      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }


    /// <summary>
    /// Deletes the employee
    /// </summary>
    public async Task<bool> DeleteEmployee(int id)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing employee
      var employee = await this._db.Employees.FirstOrDefaultAsync(i => i.Id == id);
      if (employee == null) { throw new QueryException("The employee was not found."); }

      // Delete the employee
      employee.UpdatedAt = now;
      employee.DeletedAt = now;
      this._db.Update(employee);

      // Write to system log
      this._logs.Write(
        source: EventSource.EMPLOYEE,
        type: EventType.DELETE,
        details: new { id = id },
        date: now
      );

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }


    /// <summary>
    /// Deletes multiple employees
    /// </summary>
    public async Task<bool> DeleteMultipleEmployees(List<int> ids)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      var existingEmployees = await this._db.Employees.Where(e => ids.Contains(e.Id)).ToListAsync();

      foreach (var employee in existingEmployees)
      {
        // Delete the employee
        employee.UpdatedAt = now;
        employee.DeletedAt = now;
        this._db.Update(employee);

        // Write to system log
        this._logs.Write(
          source: EventSource.EMPLOYEE,
          type: EventType.DELETE,
          details: new { id = employee.Id },
          date: now
        );
      }

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }
  }
}