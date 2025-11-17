using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

[DbContext(typeof(CorebizDataContext))]
internal class ConfigurationEntityConfiguration : IEntityTypeConfiguration<Configuration>
{
	public void Configure(EntityTypeBuilder<Configuration> builder)
	{
		builder.ToTable("configuration");

		builder.HasKey(x => x.Id);

		builder.HasIndex(t => t.TeamId)
		       .HasDatabaseName("configuration_idx_team_id");
		builder.HasIndex(t => t.ProjectId)
		       .HasDatabaseName("configuration_idx_project_id");
		builder.HasIndex(t => t.Name)
		       .HasDatabaseName("configuration_idx_name");
		builder.HasIndex(t => t.Status)
		       .HasDatabaseName("configuration_idx_status");

		builder.HasIndex(t => new { t.TeamId, t.ProjectId, t.Name })
		       .HasDatabaseName("configuration_idx_unique")
		       .IsUnique();

		builder.SnowflakeId();

		builder.Property(x => x.TeamId)
		       .HasColumnName("team_id")
		       .IsRequired();

		builder.Property(x => x.ProjectId)
		       .HasColumnName("project_id")
		       .IsRequired();

		builder.Property(x => x.Name)
		       .HasColumnName("name")
		       .HasMaxLength(255)
		       .IsRequired()
		       .IsUnicode();

		builder.Property(x => x.Description)
		       .HasColumnName("description")
		       .HasMaxLength(1024)
		       .IsUnicode();
	}
}