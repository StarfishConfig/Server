using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

internal class SupportModelBuilder : IModelBuilder
{
    public const string Key = nameof(SupportModelBuilder);

    public void Configure(ModelBuilder modelBuilder)
    {
    }
}