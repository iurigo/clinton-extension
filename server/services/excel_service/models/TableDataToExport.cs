using System.Collections.Generic;
using server.services.properties_service.models;

namespace server.services.excel_service.models
{
  public class TableDataToExport
  {
    public string TableName { get; set; }
    public List<WorksheetDataToExport> Data { get; set; }
  }
}