using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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