using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the Team entity.
/// </summary>
[DbContext(typeof(IdentityDataContext))]
internal sealed class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    /// <summary>
    /// Configures the entity of type <see cref="Team"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("team");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Name).HasDatabaseName("team_idx_name");
        builder.HasIndex(t => t.OwnerId).HasDatabaseName("team_idx_owner_id");

        builder.SnowflakeId();

        builder.Property(t => t.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(128)
               .IsUnicode();

        builder.Property(t => t.Description)
               .HasColumnName("description")
               .HasMaxLength(2000)
               .IsUnicode();

        builder.Property(t => t.OwnerId)
               .HasColumnName("owner_id")
               .IsRequired();

        builder.Property(t => t.MemberCount)
               .HasColumnName("member_count")
               .HasDefaultValue(0);

        builder.Property(t => t.ProjectCount)
               .HasColumnName("project_count")
               .HasDefaultValue(0);

        builder.ConfigureAuditableProperties();

        builder.HasMany(t => t.Members)
               .WithOne(t => t.Team)
               .HasForeignKey(t => t.TeamId);
    }
}