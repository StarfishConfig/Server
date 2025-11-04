using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the identity models for the application.
/// </summary>
internal class IdentityModelBuilder : IModelBuilder
{
    public void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("user");
            builder.HasKey(x => x.Id);
            builder.HasIndex(t => t.Username).IsUnique().HasDatabaseName("idx_username");
            builder.HasIndex(t => t.Email).IsUnique().HasDatabaseName("idx_email");
            builder.HasIndex(t => t.Phone).IsUnique().HasDatabaseName("idx_phone");

            builder.Property(x => x.Id).HasColumnName("id")
                   .HasValueGenerator<SnowflakeIdValueGenerator>()
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Username)
                   .HasColumnName("username")
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(200);
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(500);
        });
    }
}