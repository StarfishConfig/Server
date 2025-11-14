using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the TeamMember entity.
/// </summary>
[DbContext(typeof(IdentityDataContext))]
internal sealed class TeamMemberEntityConfiguration : IEntityTypeConfiguration<TeamMember>
{
    /// <summary>
    /// Configures the entity of type <see cref="TeamMember"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("team_member");
        builder.HasKey(t => t.Id);

        builder.HasIndex([nameof(TeamMember.TeamId), nameof(TeamMember.UserId)], "team_member_idx_unique")
               .IsUnique();

        builder.SnowflakeId();

        builder.Property(t => t.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(t => t.TeamId)
               .HasColumnName("team_id")
               .IsRequired();

        builder.Property(t => t.CreateTime)
               .HasColumnName("create_time")
               .HasValueGenerator<UtcTimeValueGenerator>()
               .ValueGeneratedOnAdd();

        builder.HasOne(t => t.Team)
               .WithMany(t => t.Members)
               .HasForeignKey(t => t.TeamId);

        builder.HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserId);
    }
}