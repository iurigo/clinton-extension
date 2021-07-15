using System.Collections.Generic;
using server.graphql.mutation.types.employee;
using server.services.import_service.models;

namespace server.services.import_service.models
{
  public class EmployeeDataToImport
  {
    public List<EmployeeImport> EmployeesToAdd { get; set; }
    public List<EmployeeImport> EmployeesToUpdate { get; set; }
    public List<EmployeeImport> EmployeesToRemove { get; set; }
  }
}