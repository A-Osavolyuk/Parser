namespace Parser.Models;

public sealed class RawToken
{
    public required string Rank { get; set; }
    public required string Name { get; set; }
    public required string Symbol { get; set; }
    public required string Price { get; set; }
    public required string MarketCap { get; set; }
    public required string Volume24H { get; set; }
    public required string PercentChange24H { get; set; }
}