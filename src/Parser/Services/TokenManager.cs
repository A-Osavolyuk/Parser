using Microsoft.EntityFrameworkCore;
using Parser.Common.Extensions;
using Parser.Data;
using Parser.Data.Entities;

namespace Parser.Services;

public sealed class TokenManager(AppDbContext context) : ITokenManager
{
    private readonly AppDbContext _context = context;

    public async ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tokens.AnyAsync(cancellationToken);
    }

    public async ValueTask<List<TokenEntity>> GetByQueryAsync(
        TokenFilter filter,
        int offset,
        int limit,
        string orderBy,
        string? searchAfterToken,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tokens.AsQueryable().Filter(filter);

        if (!string.IsNullOrEmpty(searchAfterToken))
        {
            query = query
                .OrderBy(x => x.Name)
                .Where(x => string.Compare(x.Name, searchAfterToken) > 0)
                .Take(limit);
        }
        else
        {
            query = query
                .ApplyOrdering(orderBy)
                .Skip(offset)
                .Take(limit);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async ValueTask<Result> CreateAsync(IEnumerable<TokenEntity> tokens,
        CancellationToken cancellationToken = default)
    {
        await _context.Tokens.AddRangeAsync(tokens, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}