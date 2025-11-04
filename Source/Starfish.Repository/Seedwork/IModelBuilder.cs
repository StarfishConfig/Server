using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines a model builder for configuring the Entity Framework Core model.
/// </summary>
public interface IModelBuilder
{
    /// <summary>
    /// Configure the model builder.
    /// </summary>
    /// <param name="modelBuilder"></param>
    void Configure(ModelBuilder modelBuilder);
}