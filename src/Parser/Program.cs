using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Data.Entities;
using Parser.Data.Extensions;
using Parser.Mapping;
using Parser.Models;
using Parser.Parsing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddOutputCache();
builder.Services.AddSingleton<ITokenParser, CoinMarketCapTokenParser>();
builder.Services.AddTransient<IMapper<RawToken, TokenEntity>, TokenMapper>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=app.db");
});

var app = builder.Build();

app.UseOutputCache();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.ConfigureDatabaseAsync<AppDbContext>();

app.Run();