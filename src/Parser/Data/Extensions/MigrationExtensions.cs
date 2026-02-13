using Microsoft.EntityFrameworkCore;

namespace Parser.Data.Extensions;

public static class MigrationExtensions
{
    public static async Task ConfigureDatabaseAsync<TDbContext>(this WebApplication app) where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        var context = serviceProvider.GetRequiredService<TDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
        
        try
        {
            logger.LogInformation("Migrating database schema");
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrated database schema");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while migrating the database");
        }
    }
}