using Microsoft.EntityFrameworkCore;
using Parser;
using Parser.Configurations;
using Parser.Data;
using Parser.Data.Entities;
using Parser.Data.Extensions;
using Parser.Mapping;
using Parser.Mediator;
using Parser.Models;
using Parser.Parsing;
using Parser.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddOutputCache();
builder.Services.AddProblemDetails();
builder.Services.AddMediator<IAssemblyMarker>();
builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddSingleton<ITokenParser, CoinMarketCapTokenParser>();
builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddTransient<IMapper<RawToken, TokenEntity>, TokenMapper>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=app.db");
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseOutputCache();
app.UseHttpsRedirection();
app.MapControllers();

await app.ConfigureDatabaseAsync<AppDbContext>();

app.Run();