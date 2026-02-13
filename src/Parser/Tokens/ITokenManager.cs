using Parser.Data.Entities;
using Parser.Filtering;
using Parser.Results;

namespace Parser.Tokens;

public interface ITokenManager
{
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