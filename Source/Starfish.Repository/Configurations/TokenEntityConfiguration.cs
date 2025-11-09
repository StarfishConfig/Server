using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the Token entity.
/// </summary>
[DbContext(typeof(AccountDataContext))]
internal sealed class TokenEntityConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("token");

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Key).HasDatabaseName("token_idx_key");
        builder.HasIndex(t => t.Expires).HasDatabaseName("token_idx_expires");

        builder.Property(t => t.Id)
               .HasColumnName("id")
               .IsRequired()
               .HasValueGenerator<SnowflakeIdValueGenerator>();

        builder.Property(t => t.Type)
               .HasColumnName("type")
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(t => t.Key)
               .HasColumnName("key")
               .IsRequired()
               .HasMaxLength(64);

        builder.Property(t => t.Subject)
               .HasColumnName("subject")
               .IsRequired();

        builder.Property(t => t.Expires)
               .HasColumnName("expires")
               .IsRequired();

        builder.Property(t => t.Issues)
               .HasColumnName("issues")
               .IsRequired();
    }
}