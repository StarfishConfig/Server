using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// User Role Aggregate
/// </summary>
internal class UserRole : Entity<long>, IHasCreateTime
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private UserRole()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="name"></param>
    private UserRole(string name)
    {
        Name = name;
    }


    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name { get; set; }

    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Creates a new <see cref="UserRole"/> entity instance with specified name.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns></returns>
    internal static UserRole Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), Resources.IDS_ERROR_ROLE_NAME_REQUIRED);
        }
        name = name.Trim().ToLowerInvariant();
        return new UserRole(name);
    }
}
