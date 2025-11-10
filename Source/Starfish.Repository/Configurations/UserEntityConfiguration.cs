using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the User entity.
/// </summary>
[DbContext(typeof(AccountDataContext))]
internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Username)
               .HasDatabaseName("user_idx_username")
               .IsUnique();

        builder.HasIndex(t => t.Email)
               .HasDatabaseName("user_idx_email")
               .IsUnique();

        builder.HasIndex(t => t.Phone)
               .HasDatabaseName("user_idx_phone")
               .IsUnique();

        builder.Property(t => t.Id)
               .HasColumnName("id")
               .HasValueGenerator<SnowflakeIdValueGenerator>()
               .IsRequired();

        builder.Property(t => t.Username)
               .HasColumnName("username")
               .IsRequired()
               .HasMaxLength(64);

        builder.Property(t => t.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired()
               .HasMaxLength(512);

        builder.Property(t => t.PasswordSalt)
               .HasColumnName("password_salt")
               .IsRequired()
               .HasMaxLength(32);

        builder.Property(t => t.Email)
               .HasColumnName("email")
               .HasMaxLength(255);

        builder.Property(t => t.Phone)
               .HasColumnName("phone")
               .HasMaxLength(32);

        builder.Property(t => t.Nickname)
               .HasColumnName("nickname")
               .IsUnicode();

        builder.Property(t => t.AccessFailedCount)
               .HasColumnName("access_failed_count")
               .HasDefaultValue(0);

        builder.Property(t => t.LockoutEnd)
               .HasColumnName("lockout_end");

        builder.Property(t => t.Source)
               .HasColumnName("source");

        builder.Property(t => t.CreateTime)
               .HasColumnName("create_time")
               .HasValueGenerator<UtcTimeValueGenerator>()
               .ValueGeneratedOnAdd();

        builder.Property(t => t.UpdateTime)
               .HasColumnName("update_time")
               .HasValueGenerator<UtcTimeValueGenerator>()
               .ValueGeneratedOnAddOrUpdate();

        builder.Property(t => t.IsDeleted)
               .HasColumnName("is_deleted")
               .HasDefaultValue(false);

        builder.Property(t => t.DeleteTime)
               .HasColumnName("delete_time");

        builder.HasMany(x => x.Roles)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}