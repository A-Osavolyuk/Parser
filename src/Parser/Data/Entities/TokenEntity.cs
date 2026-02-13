namespace Parser.Data.Entities;

public sealed class TokenEntity
{
    public Guid Id { get; set; }

    public required int Rank { get; set; }
    public required string Name { get; set; }
    public required string Symbol { get; set; }
    public required decimal Price { get; set; }
    public required decimal MarketCap { get; set; }
    public required float PercentChange24H { get; set; }
    public long? Volume24H { get; set; }
}