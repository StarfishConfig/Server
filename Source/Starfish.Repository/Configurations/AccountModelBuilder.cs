using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the identity models for the application.
/// </summary>
internal class AccountModelBuilder : IModelBuilder
{
    public const string Key = nameof(AccountModelBuilder);

    public void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.Username)
                  .HasDatabaseName("user_idx_username")
                  .IsUnique();

            entity.HasIndex(t => t.Email)
                  .HasDatabaseName("user_idx_email")
                  .IsUnique();

            entity.HasIndex(t => t.Phone)
                  .HasDatabaseName("user_idx_phone")
                  .IsUnique();

            entity.Property(t => t.Id)
                  .HasColumnName("id")
                  .HasValueGenerator<SnowflakeIdValueGenerator>()
                  .IsRequired();

            entity.Property(t => t.Username)
                  .HasColumnName("username")
                  .IsRequired()
                  .HasMaxLength(64);

            entity.Property(t => t.PasswordHash)
                  .HasColumnName("password_hash")
                  .IsRequired()
                  .HasMaxLength(512);

            entity.Property(t => t.PasswordSalt)
                  .HasColumnName("password_salt")
                  .IsRequired()
                  .HasMaxLength(32);

            entity.Property(t => t.Email)
                  .HasColumnName("email")
                  .HasMaxLength(255);

            entity.Property(t => t.Phone)
                  .HasColumnName("phone")
                  .HasMaxLength(32);

            entity.Property(t => t.Nickname)
                  .HasColumnName("nickname")
                  .IsUnicode();

            entity.Property(t => t.AccessFailedCount)
                  .HasColumnName("access_failed_count")
                  .HasDefaultValue(0);

            entity.Property(t => t.LockoutEnd)
                  .HasColumnName("lockout_end");

            entity.Property(t => t.Source)
                  .HasColumnName("source");

            entity.Property(t => t.CreateTime)
                  .HasColumnName("create_time")
                  .HasValueGenerator<UtcTimeValueGenerator>()
                  .ValueGeneratedOnAdd();

            entity.Property(t => t.UpdateTime)
                  .HasColumnName("update_time")
                  .HasValueGenerator<UtcTimeValueGenerator>()
                  .ValueGeneratedOnAddOrUpdate();

            entity.Property(t => t.IsDeleted)
                  .HasColumnName("is_deleted")
                  .HasDefaultValue(false);

            entity.Property(t => t.DeleteTime)
                  .HasColumnName("delete_time");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("team");
            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.Name).HasDatabaseName("team_idx_name");
            entity.HasIndex(t => t.OwnerId).HasDatabaseName("team_idx_owner_id");

            entity.Property(t => t.Id)
                  .HasColumnName("id")
                  .IsRequired()
                  .HasValueGenerator<SnowflakeIdValueGenerator>()
                  .ValueGeneratedOnAdd();

            entity.Property(t => t.Name)
                  .HasColumnName("name")
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode();

            entity.Property(t => t.Description)
                  .HasColumnName("description")
                  .HasMaxLength(2000)
                  .IsUnicode();

            entity.Property(t => t.OwnerId)
                  .HasColumnName("owner_id")
                  .IsRequired();

            entity.Property(t => t.MemberCount)
                  .HasColumnName("member_count")
                  .HasDefaultValue(0);

            entity.Property(t => t.ProjectCount)
                  .HasColumnName("project_count")
                  .HasDefaultValue(0);

            entity.HasMany(t => t.Members)
                  .WithOne(t => t.Team)
                  .HasForeignKey(t => t.TeamId);
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.ToTable("team_member");
            entity.HasKey(t => t.Id);

            entity.HasIndex([nameof(TeamMember.TeamId), nameof(TeamMember.UserId)], "team_member_idx_unique")
                  .IsUnique();

            entity.Property(t => t.Id)
                  .HasColumnName("id")
                  .IsRequired()
                  .HasValueGenerator<SnowflakeIdValueGenerator>()
                  .ValueGeneratedOnAdd();

            entity.Property(t => t.UserId)
                  .HasColumnName("user_id")
                  .IsRequired();

            entity.Property(t => t.TeamId)
                  .HasColumnName("team_id")
                  .IsRequired();

            entity.Property(t => t.CreateTime)
                  .HasColumnName("create_time")
                  .HasValueGenerator<UtcTimeValueGenerator>()
                  .ValueGeneratedOnAdd();

            entity.HasOne(t => t.Team)
                  .WithMany(t => t.Members)
                  .HasForeignKey(t => t.TeamId);

            entity.HasOne(t => t.User)
                  .WithMany()
                  .HasForeignKey(t => t.UserId);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.ToTable("token");

            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.Key).HasDatabaseName("token_idx_key");
            entity.HasIndex(t => t.Expires).HasDatabaseName("token_idx_expires");

            entity.Property(t => t.Id)
                  .HasColumnName("id")
                  .IsRequired()
                  .HasValueGenerator<SnowflakeIdValueGenerator>();

            entity.Property(t => t.Type)
                  .HasColumnName("type")
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(t => t.Key)
                  .HasColumnName("key")
                  .IsRequired()
                  .HasMaxLength(64);

            entity.Property(t => t.Subject)
                  .HasColumnName("subject")
                  .IsRequired();

            entity.Property(t => t.Expires)
                  .HasColumnName("expires")
                  .IsRequired();

            entity.Property(t => t.Issues)
                  .HasColumnName("issues")
                  .IsRequired();
        });
    }
}