using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Represents a third-party user authority linked to a user.
/// </summary>
public sealed class UserAuthority : Entity<long>, IHasCreateTime
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private UserAuthority()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthority"/> class.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="openId"></param>
    private UserAuthority(string provider, string openId)
        : this()
    {
        Provider = provider;
        OpenId = openId;
    }

    /// <summary>
    /// Gets or sets the user ID associated with this authority.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the provider of the third-party authentication.
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user on the third-party platform.
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user on the third-party platform.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the creation time of this authority record.
    /// </summary>
    public DateTime CreateTime { get; set; }

    #region Associates

    /// <summary>
    /// Gets or sets the user associated with this authority.
    /// </summary>
    public User User { get; set; }

    #endregion

    #region Metods

    /// <summary>
    /// Creates a new instance of <see cref="UserAuthority"/>.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="openId"></param>
    /// <returns></returns>
    internal static UserAuthority Create(string provider, string openId)
    {
        provider = provider.Trim().ToLowerInvariant();
        return new UserAuthority(provider, openId);
    }

    /// <summary>
    /// Sets the name of the user.
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    internal void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), Resources.IDS_ERROR_EXTERNAL_NAME_REQUIRED);
        }

        Name = name.Trim();
    }

    #endregion
}