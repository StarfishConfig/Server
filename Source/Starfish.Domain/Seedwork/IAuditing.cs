using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Auditing interface for entities that track creation and update information.
/// </summary>
public interface IAuditing : IHasCreateTime, IHasUpdateTime
{
    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the entity.
    /// </summary>
    string UpdatedBy { get; set; }
}