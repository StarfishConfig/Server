using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

internal class ProjectModelBuilder : IModelBuilder
{
    public const string Key = nameof(ProjectModelBuilder);

    /// <inheritdoc/>
    public void Configure(ModelBuilder modelBuilder)
    {
    }
}