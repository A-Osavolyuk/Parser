using Microsoft.EntityFrameworkCore;
using Parser.Data.Entities;

namespace Parser.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TokenEntity> Tokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}