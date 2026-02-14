using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Parser.Common.Filtering;
using Parser.Services;

namespace Parser.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TokensController(ITokenManager tokenManager) : ControllerBase
{
    private readonly ITokenManager _tokenManager = tokenManager;

    [HttpGet]
    [OutputCache(Duration = 60)]
    public async ValueTask<IActionResult> GetAsync(
        [FromQuery] TokenFilter filter,
        [FromQuery(Name = "order_by")] string orderBy = "rank",
        [FromQuery(Name = "search_after_token")] string? searchAfterToken = null,
        [FromQuery(Name = "offset")] int offset = 0,
        [FromQuery(Name = "limit")] int limit = 100)
    {
        var tokens = await _tokenManager.GetByQueryAsync(
            filter, offset, limit, orderBy, searchAfterToken);

        return Ok(tokens);
    }
}