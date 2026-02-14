using Parser.Common.Extensions;
using Parser.Common.Models;
using Parser.Common.Parsing;
using Parser.Data.Entities;
using Parser.Mapping;
using Parser.Services;

namespace Parser.Features.Tokens.Queries;

public sealed record GetTokensQuery(
    TokenFilter Filter,
    int Offset,
    int Limit,
    string OrderBy,
    string? SearchAfterToken) : IRequest<Result>;

public sealed class GetTokenQueryHandler(
    ITokenManager tokenManager,
    ITokenParser tokenParser,
    IMapper<RawToken, TokenEntity> mapper,
    ILogger<GetTokenQueryHandler> logger) : IRequestHandler<GetTokensQuery, Result>
{
    private readonly ITokenManager _tokenManager = tokenManager;
    private readonly ITokenParser _tokenParser = tokenParser;
    private readonly IMapper<RawToken, TokenEntity> _mapper = mapper;
    private readonly ILogger<GetTokenQueryHandler> _logger = logger;

    public async ValueTask<Result> HandleAsync(GetTokensQuery request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Attempting to load tokens from the database");
        var tokens = await _tokenManager.GetByQueryAsync(request.Filter, request.Offset,
            request.Limit, request.OrderBy, request.SearchAfterToken, cancellationToken);

        if (tokens.Count > 0 || await _tokenManager.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Retrieving successfully loaded tokens from the database");
            return Result.Success(tokens);
        }

        _logger.LogInformation("Initiating tokens parsing");
        var rawTokens = await _tokenParser.ParseAsync(cancellationToken);
        if (rawTokens.Count == 0)
        {
            _logger.LogError("Error on parsing a data source");
            return Result.Fail(new Error()
            {
                Code = "Server error",
                Description = "Something went wrong on parsing data"
            });
        }

        var entities = rawTokens.Select(_mapper.Map).DistinctBy(x => x.Name).ToList();
        var creationResult = await _tokenManager.CreateAsync(entities, cancellationToken);
        if (!creationResult.IsSucceeded)
        {
            _logger.LogError("Error on saving tokens to the database");
            return creationResult;
        }

        var query = entities.AsQueryable().Filter(request.Filter);
        if (!string.IsNullOrEmpty(request.SearchAfterToken))
        {
            query = query
                .OrderBy(x => x.Name)
                .Where(x => string.Compare(x.Name, request.SearchAfterToken) > 0)
                .Take(request.Limit);
        }
        else
        {
            query = query
                .ApplyOrdering(request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit);
        }

        _logger.LogInformation("Retrieving successfully parsed tokens from a data source");
        return Result.Success(query);
    }
}