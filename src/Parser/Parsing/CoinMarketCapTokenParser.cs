using Microsoft.Playwright;
using Parser.Models;

namespace Parser.Parsing;

public sealed class CoinMarketCapTokenParser(
    ILogger<CoinMarketCapTokenParser> logger) : ITokenParser
{
    private readonly ILogger<CoinMarketCapTokenParser> _logger = logger;

    public async ValueTask<List<RawToken>> ParseAsync(CancellationToken cancellationToken = default)
    {
        var playwright = await Playwright.CreateAsync();

        _logger.LogInformation("Creating browser instance");
        var options = new BrowserTypeLaunchOptions { Headless = true };
        await using var browser = await playwright.Chromium.LaunchAsync(options);
        _logger.LogInformation("Created browser instance");
        
        _logger.LogInformation("Creating new page");
        var page = await browser.NewPageAsync();

        _logger.LogInformation("Navigating to source page");
        await page.GotoAsync("https://coinmarketcap.com/all/views/all");
        
        _logger.LogInformation("Cleaning-up redundant styling on page");
        await page.EvaluateAsync(@"
            () => {
                // remove all stylesheets
                document.querySelectorAll('link[rel=stylesheet], style')
                    .forEach(el => el.remove());
            
                // remove inline styles
                document.querySelectorAll('*')
                    .forEach(el => el.removeAttribute('style'));
            }");
        
        _logger.LogInformation("Setting page zoom to 1%");
        await page.EvaluateAsync("() => { document.body.style.zoom = '10%'; }");

        _logger.LogInformation("Selecting source rows");
        await page.WaitForSelectorAsync("tbody tr.cmc-table-row");
        var seen = new HashSet<string>();
        var rawTokens = new List<RawToken>();

        _logger.LogInformation("Begin processing source rows");
        while (true)
        {
            var rows = await page.Locator("tbody tr.cmc-table-row").AllAsync();
            var newRows = 0;

            foreach (var row in rows)
            {
                try
                {
                    await row.ScrollIntoViewIfNeededAsync();
                    
                    var rank = await row
                        .Locator("td.cmc-table__cell--sort-by__rank")
                        .InnerTextAsync(new LocatorInnerTextOptions { Timeout = 2000 });

                    if (!seen.Add(rank))
                        continue;

                    newRows++;
                    
                    var name = await row
                        .Locator("td.cmc-table__cell--sort-by__name")
                        .Locator("div a.cmc-table__column-name--name.cmc-link")
                        .InnerTextAsync();

                    string? marketCap;
                    try
                    {
                        marketCap = await row
                            .Locator("td.cmc-table__cell--sort-by__market-cap")
                            .Locator("span[data-nosnippet='true']")
                            .InnerTextAsync();
                    }
                    catch (Exception e)
                    {
                        marketCap = null;
                    }
            
                    var volume24H = await row.Locator("td.cmc-table__cell--sort-by__volume-24-h")
                        .InnerTextAsync();
                    
                    var percentChange24H = await row.Locator("td.cmc-table__cell--sort-by__percent-change-24-h")
                        .InnerTextAsync();
                    
                    var symbol = await row.Locator("td.cmc-table__cell--sort-by__symbol")
                        .InnerTextAsync();
                    
                    var price = await row.Locator("td.cmc-table__cell--sort-by__price")
                        .InnerTextAsync();

                    rawTokens.Add(new RawToken()
                    {
                        Rank = rank,
                        Name = name,
                        Symbol = symbol,
                        Price = price,
                        MarketCap = marketCap,
                        Volume24H = volume24H,
                        PercentChange24H = percentChange24H,
                    });

                    await row.EvaluateAsync("el => el.classList.add('processed')");
                }
                catch
                {
                    _logger.LogError("Couldn't parse {Locator}", row);
                }
            }
            
            if (newRows == 0)
                break;
            
            await page.EvaluateAsync(@"
                () => {
                    document.querySelectorAll('tbody tr.processed')
                    .forEach(r => r.remove());
                }");

            var button = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Load More" });

            if (!await button.IsVisibleAsync())
                break;

            await button.ClickAsync();
            await page.WaitForTimeoutAsync(2000);
            
            _logger.LogInformation("Successfully proceeded {rowsCount} rows", rawTokens.Count);
        }

        _logger.LogInformation("Source rows were successfully proceeded");
        return rawTokens;
    }
}