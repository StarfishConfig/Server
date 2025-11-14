using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

[DbContext(typeof(PrimaryDataContext))]
internal class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		throw new NotImplementedException();
	}
}
