using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parser.Data.Entities;

namespace Parser.Data.Configuration;

public sealed class TokenConfiguration : IEntityTypeConfiguration<TokenEntity>
{
    public void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Symbol).HasMaxLength(50);

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Token_Name")
            .IsUnique();
    }
}