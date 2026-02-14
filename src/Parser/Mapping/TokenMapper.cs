using System.Text;
using Parser.Common.Models;
using Parser.Data.Entities;

namespace Parser.Mapping;

public sealed class TokenMapper : IMapper<RawToken, TokenEntity>
{
    public TokenEntity Map(RawToken input)
    {
        var cleanedPrice = new StringBuilder(input.Price)
            .Replace("$", "")
            .Replace(",", "")
            .Replace(".", ",")
            .ToString();
            
        var cleanedMarketCap = new StringBuilder(input.MarketCap)
            .Replace("$", "")
            .Replace(",", "")
            .ToString();
            
        var cleanedVolume24H = new StringBuilder(input.Volume24H)
            .Replace('\u00A0', ' ')
            .Replace('\u202F', ' ')
            .Replace("$", "")
            .Replace(" ", "")
            .ToString();
            
        var cleanedPercentChange24H = new StringBuilder(input.PercentChange24H)
            .Replace("%", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace(".", ",")
            .ToString();
        
        return new TokenEntity()
        {
            Id = Guid.NewGuid(),
            Rank = int.Parse(input.Rank),
            Name = input.Name,
            Symbol = input.Symbol,
            Price = decimal.Parse(cleanedPrice),
            MarketCap = decimal.TryParse(cleanedMarketCap, out var marketCap) ? marketCap : null,
            PercentChange24H = float.Parse(cleanedPercentChange24H),
            Volume24H = long.TryParse(cleanedVolume24H, out var volume) ? volume : null,
        };
    }
}