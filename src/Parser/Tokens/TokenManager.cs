using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Data.Entities;
using Parser.Extensions;
using Parser.Filtering;
using Parser.Results;

namespace Parser.Tokens;

public sealed class TokenManager(AppDbContext context) : ITokenManager
{
    private readonly AppDbContext _context = context;

    public async ValueTask<PageableResult<TokenEntity>> GetByQueryAsync(
        TokenFilter filter,
        int offset, 
        int limit, 
        string orderBy,
        CancellationToken cancellationToken = default)
    {
        var totalItems = await _context.Tokens.CountAsync(cancellationToken);
        var items = await _context.Tokens.AsQueryable()
            .Filter(filter)
            .ApplyOrdering(orderBy)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
        
        return new PageableResult<TokenEntity>()
        {
            Offset =  offset,
            Limit = limit,
            Items = items,
            TotalItems = totalItems
        };
    }

    public async ValueTask<Result> CreateAsync(IEnumerable<TokenEntity> tokens,
        CancellationToken cancellationToken = default)
    {
        await _context.Tokens.AddRangeAsync(tokens, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}