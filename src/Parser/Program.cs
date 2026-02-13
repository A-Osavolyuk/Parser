using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Data.Extensions;
using Parser.Parsing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITokenParser, CoinMarketCapTokenParser>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=app.db");
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.ConfigureDatabaseAsync<AppDbContext>();

app.Run();