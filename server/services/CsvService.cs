using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;

namespace server.services.csv
{
  public interface ICsvService
  {
    DataTable Read(byte[] data, string name = "Default");
    byte[] Write(DataTable dataTable);
  }


  public class CsvService : ICsvService
  {
    public DataTable Read(byte[] data, string name = "Default")
    {
      using (var reader = new StringReader(Encoding.UTF8.GetString(data)))
      using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
      using (var dataReader = new CsvDataReader(csv))
      {
        // Read data table
        var dataTable = new DataTable(name);
        dataTable.Load(dataReader);

        // Return data table
        return dataTable;
      }
    }

    public byte[] Write(DataTable dataTable)
    {
      using (var writer = new StringWriter())
      using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
      {
        // Write column names
        foreach (DataColumn column in dataTable.Columns)
        {
          csv.WriteField(column.ColumnName);
        }
        csv.NextRecord();

        // Write data rows
        foreach (DataRow row in dataTable.Rows)
        {
          for (int i = 0; i < dataTable.Columns.Count; i++)
          {
            csv.WriteField(row[i]);
          }
          csv.NextRecord();
        }

        // Return generated CSV
        return Encoding.UTF8.GetBytes(writer.ToString());
      }
    }

  }
}