using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the UserAuthority entity.
/// </summary>
[DbContext(typeof(AccountDataContext))]
internal class UserAuthorityEntityConfiguration : IEntityTypeConfiguration<UserAuthority>
{
    /// <summary>
    /// Configures the entity of type <see cref="UserAuthority"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<UserAuthority> builder)
    {
        builder.ToTable("user_authority");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId)
               .HasDatabaseName("user_authority_idx_user_id");

        builder.HasIndex(x => new { x.Provider, x.OpenId })
               .HasDatabaseName("user_authority_idx_unique")
               .IsUnique();

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .HasValueGenerator<SnowflakeIdValueGenerator>()
               .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(x => x.Provider)
               .HasColumnName("provider")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.OpenId)
               .HasColumnName("open_id")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasColumnName("name")
               .HasMaxLength(255)
               .IsRequired(false);

        builder.Property(x => x.CreateTime)
               .HasColumnName("created_at")
               .HasDefaultValue(DateTime.Now)
               .ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Authorities)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}