using System.Text.Json.Serialization;

namespace Parser.Results;

public sealed class PageableResult<T> where T : new()
{
    [JsonPropertyName("items")]
    public IEnumerable<T> Items { get; set; } = new List<T>();
    
    [JsonPropertyName("total_items")]
    public int TotalItems { get; set; }
    
    [JsonPropertyName("page_number")]
    public int PageNumber { get; set; }
    
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }
    
    [JsonPropertyName("total_pages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}