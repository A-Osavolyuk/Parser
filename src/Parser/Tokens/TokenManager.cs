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
            query = query
                .OrderBy(x => x.Name)
                .Where(x => string.Compare(x.Name, searchAfterToken) > 0)
                .Take(limit);
        else
            query = query
                .ApplyOrdering(orderBy)
                .Skip(offset)
                .Take(limit);

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