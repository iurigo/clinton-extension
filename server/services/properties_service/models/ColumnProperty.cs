namespace server.services.properties_service.models
{
  public class ColumnProperty
  {
    public string Name { get; set; }
    public ColumnType Type { get; set; }
    public int ColumnNumber { get; set; }
    public string Format { get; set; }
  }
}