using Parser.Models;

namespace Parser.Parsing;

public interface ITokenParser
{
    public ValueTask<List<RawToken>> ParseAsync(CancellationToken cancellationToken = default);
}