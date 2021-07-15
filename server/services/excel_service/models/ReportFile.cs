using System.Collections.Generic;

namespace server.services.excel_service.models
{
  public class ReportFile
  {
    public string Name { get; set; }
    public byte[] Data { get; set; }
  }
}