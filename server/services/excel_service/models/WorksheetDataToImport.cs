using System.Collections.Generic;

namespace server.services.excel_service.models
{
  public class WorksheetDataToImport
  {
    public string WorksheetName { get; set; }
    public List<List<CellValue>> Data { get; set; }
  }
}