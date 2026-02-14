namespace Parser.Common.Filtering;

public sealed class TokenFilter
{
    public string? Name { get; set; }
    public string? Symbol { get; set; }
    public int? Rank { get; set; }
    public int? MinRank { get; set; }
    public int? MaxRank { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MaxMarketCap { get; set; }
    public decimal? MinMarketCap { get; set; }
    public long? MinVolume { get; set; }
    public long? MaxVolume { get; set; }
    public float? MinPercentChange { get; set; }
    public float? MaxPercentChange { get; set; }
}