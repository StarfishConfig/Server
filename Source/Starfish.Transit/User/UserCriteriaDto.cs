using System.ComponentModel.DataAnnotations;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for user criteria.
/// </summary>
public class UserCriteriaDto
{
    /// <summary>
    /// Gets or sets the keyword for searching users.
    /// </summary>
    /// <remarks>
    /// This keyword can be used to search by username, nickname, or other relevant fields.
    /// </remarks>
    public string Keyword { get; set; }

    /// <summary>
    /// Gets or sets the source of user creation.
    /// </summary>
    [AllowedValues(UserCreationSource.InitialImport, UserCreationSource.AdminCreated, UserCreationSource.SelfRegistered)]
    public int? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter users by their locked status.
    /// </summary>
    public bool? Locked { get; set; }
}