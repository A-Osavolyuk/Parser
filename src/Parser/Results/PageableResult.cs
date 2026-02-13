using System.Text.Json.Serialization;

namespace Parser.Results;

public sealed class PageableResult<T> where T : notnull
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    
    [JsonPropertyName("total_items")]
    public int TotalItems { get; set; }
    
    [JsonPropertyName("items")]
    public IEnumerable<T> Items { get; set; } = new List<T>();
}