using server.database.enums;

namespace server.services.import_service.models
{
  public class EmployeeImport
  {
    // Properties
    public int? Id { get; set; }
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmployeeDiscipline Discipline { get; set; }
    public float? Rate { get; set; }
  }
}