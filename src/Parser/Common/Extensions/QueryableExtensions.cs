using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Parser.Data.Entities;

namespace Parser.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string propertyName)
    {
        var descending = propertyName.Contains('-');
        var purePropertyName = new StringBuilder(propertyName).Replace("-", string.Empty).ToString();
        
        var property = typeof(T).GetProperty(purePropertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        
        if (property is null)
            throw new ArgumentNullException(nameof(purePropertyName));
        
        var param = Expression.Parameter(typeof(T), purePropertyName);
        var body = Expression.PropertyOrField(param, purePropertyName);
        var selector = Expression.Lambda(body, param);
        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), body.Type],
            query.Expression,
            Expression.Quote(selector));
        
        return query.Provider.CreateQuery<T>(resultExpression);
    }
    
    public static IQueryable<TokenEntity> Filter(this IQueryable<TokenEntity> tokens, TokenFilter filter)
    {
        var query = tokens.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name == filter.Name);
        
        if (!string.IsNullOrEmpty(filter.Symbol))
            query = query.Where(x => x.Symbol == filter.Symbol);
        
        if (filter.Rank is not null)
            query = query.Where(x => x.Rank == filter.Rank);
        
        if (filter.MinRank is not null)
            query = query.Where(x => x.Rank >= filter.MinRank);
        
        if (filter.MaxRank is not null)
            query = query.Where(x => x.Rank >= filter.MaxRank);
        
        if (filter.MinPrice is not null)
            query = query.Where(x => x.Price >= filter.MinPrice);
        
        if (filter.MaxPrice is not null)
            query = query.Where(x => x.Price >= filter.MaxPrice);
        
        if (filter.MaxMarketCap is not null)
            query = query.Where(x => x.MarketCap >= filter.MaxMarketCap);
        
        if (filter.MinMarketCap is not null)
            query = query.Where(x => x.MarketCap >= filter.MinMarketCap);
        
        if (filter.MinVolume is not null)
            query = query.Where(x => x.Volume24H >= filter.MinVolume);
        
        if (filter.MaxVolume is not null)
            query = query.Where(x => x.Volume24H >= filter.MaxVolume);
        
        if (filter.MinPercentChange is not null)
            query = query.Where(x => x.PercentChange24H >= filter.MinPercentChange);
        
        if (filter.MaxPercentChange is not null)
            query = query.Where(x => x.PercentChange24H >= filter.MaxPercentChange);
        
        return query;
    }
}