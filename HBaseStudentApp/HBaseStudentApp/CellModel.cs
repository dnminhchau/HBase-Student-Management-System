using System.Collections.Generic;
using System.Text.Json.Serialization;

public class CellModel
{
    [JsonPropertyName("column")]
    public string Column { get; set; }

    [JsonPropertyName("$")]
    public string Value { get; set; }  
}

public class RowModel
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("Cell")]
    public List<CellModel> Cells { get; set; }
}

public class CellSetModel
{
    [JsonPropertyName("Row")]
    public List<RowModel> Rows { get; set; }
}
