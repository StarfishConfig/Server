using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Configures the Project entity.
/// </summary>
[DbContext(typeof(PrimaryDataContext))]
internal class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
{
	private const string TABLE = "project";

	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.ToTable(TABLE);

		builder.HasKey(e => e.Id);

		builder.HasIndex(t => t.TeamId).HasDatabaseName("project_idx_teamid");

		builder.SnowflakeId();

		builder.Property(t => t.TeamId)
			.HasColumnName("team_id")
			.IsRequired();

		builder.Property(t => t.Name)
			.HasColumnName("name")
			.HasMaxLength(128)
			.IsRequired()
			.IsUnicode();

		builder.Property(t => t.Description)
			.HasColumnName("description")
			.HasMaxLength(512)
			.IsUnicode();

		builder.Property(t => t.Url)
			.HasColumnName("url")
			.HasMaxLength(256);

		builder.Property(t => t.Image)
			.HasColumnName("image")
			.HasMaxLength(256);

		builder.ConfigureAuditableProperties();
	}
}
