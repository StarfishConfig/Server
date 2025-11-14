using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

[DbContext(typeof(IdentityDataContext))]
internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// Configures the entity of type <see cref="UserRole"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_role");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId)
               .HasDatabaseName("user_role_idx_user_id");

        builder.HasIndex(x => new { x.UserId, x.Name })
               .HasDatabaseName("user_role_idx_unique")
               .IsUnique();

        builder.SnowflakeId();

        builder.Property(x => x.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(x => x.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.CreateTime)
               .HasColumnName("created_at")
               .HasDefaultValue(DateTime.UtcNow)
               .ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}