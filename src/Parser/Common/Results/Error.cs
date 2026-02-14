using System.Text.Json.Serialization;

namespace Parser.Common.Results;

public sealed class Error
{
    [JsonPropertyName("error_code")]
    public required string Code { get; set; }
    
    [JsonPropertyName("error_description")]
    public required string Description { get; set; }
}