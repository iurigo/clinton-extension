using System.Collections.Generic;
using server.services.properties_service.models;

namespace server.services.excel_service.models
{
  public class WorksheetDataToExport
  {
    public string WorksheetName { get; set; }
    public List<ColumnProperty> Properties { get; set; }
    public List<List<object>> Data { get; set; }
  }
}