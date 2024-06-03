namespace MicroApi.Mapper;

public sealed class TableMapping
{
    public string Source => SourceType.Name;

    public string Target { get; set; }
    
    public Type SourceType { get; set; }
    
    public Dictionary<string, string> ColumnConfig { get; set; }
}
