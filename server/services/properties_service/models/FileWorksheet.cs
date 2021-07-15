using System.Collections.Generic;

namespace server.services.properties_service.models
{
  public class FileWorksheet
  {
    public string Name { get; set; }
    public List<ColumnProperty> Properties { get; set; }
  }
}