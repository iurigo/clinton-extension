using System;
using System.Collections.Generic;
using server.database.enums;

namespace server.database.models
{
  public class Employee
  {
    // Key
    public int Id { get; set; }

    // Properties
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmployeeDiscipline Discipline { get; set; }
    public float Rate { get; set; }
    public bool IsActive { get; set; }

    // Timestamps
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
  }
}