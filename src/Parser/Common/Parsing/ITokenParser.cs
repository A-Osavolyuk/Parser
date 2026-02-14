using Parser.Common.Models;

namespace Parser.Common.Parsing;

public interface ITokenParser
{
    public ValueTask<List<RawToken>> ParseAsync(CancellationToken cancellationToken = default);
}