using Parser.Data.Entities;
using Parser.Results;

namespace Parser.Tokens;

public interface ITokenManager
{
    public ValueTask<List<TokenEntity>> GetAllAsync(CancellationToken cancellationToken);
    public ValueTask<Result> CreateAsync(IEnumerable<TokenEntity> tokens, CancellationToken cancellationToken);
}