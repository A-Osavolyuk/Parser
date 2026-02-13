using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Data.Entities;
using Parser.Results;

namespace Parser.Tokens;

public sealed class TokenManager(AppDbContext context) : ITokenManager
{
    private readonly AppDbContext _context = context;

    public async ValueTask<List<TokenEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tokens.ToListAsync(cancellationToken);
    }

    public async ValueTask<Result> CreateAsync(IEnumerable<TokenEntity> tokens, CancellationToken cancellationToken)
    {
        await _context.Tokens.AddRangeAsync(tokens, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}