using Parser.Data.Entities;

namespace Parser.Services;

public interface ITokenManager
{
    public ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default);
    
    public ValueTask<List<TokenEntity>> GetByQueryAsync(
        TokenFilter filter,
        int offset,
        int limit,
        string orderBy,
        string? searchAfterToken,
        CancellationToken cancellationToken = default);
    
    public ValueTask<Result> CreateAsync(IEnumerable<TokenEntity> tokens, 
        CancellationToken cancellationToken = default);
}