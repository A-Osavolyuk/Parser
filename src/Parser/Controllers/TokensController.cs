using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Parser.Features.Tokens.Queries;

namespace Parser.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TokensController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [OutputCache(Duration = 60)]
    public async ValueTask<IActionResult> GetAsync(
        [FromQuery] TokenFilter filter,
        [FromQuery(Name = "order_by")] string orderBy = "rank",
        [FromQuery(Name = "search_after_token")] string? searchAfterToken = null,
        [FromQuery(Name = "offset")] int offset = 0,
        [FromQuery(Name = "limit")] int limit = 100)
    {
        var result = await _sender.SendAsync(new GetTokensQuery(filter, offset, limit, orderBy, searchAfterToken));
        return result.IsSucceeded ? Ok(result.Value) : StatusCode(500, result.GetError());
    }
}