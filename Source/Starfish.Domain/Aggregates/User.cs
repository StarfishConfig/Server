using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Toolkit;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the user information aggregate.
/// </summary>
internal sealed class User : Aggregate<long>, IHasCreateTime, IHasUpdateTime, ITombstone
{
    #region Ctors

    /// <summary>
    /// Prevents a default instance of the <see cref="User"/> class from being created.
    /// </summary>
    private User()
    {
        Register<UserEmailChangedEvent>(@event =>
        {
            Email = @event.NewValue.Normalize(TextCaseType.Lower);
        });
        Register<UserPhoneChangedEvent>(@event =>
        {
            Phone = @event.NewValue.Normalize(TextCaseType.Lower);
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="username"></param>
    private User(string username)
        : this()
    {
        Username = username;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password hash stored securely.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the password salt used for hashing.
    /// </summary>
    public string PasswordSalt { get; set; }

    /// <summary>
    /// Gets or sets the time when the password was last changed.
    /// </summary>
    public DateTime? PasswordChangedTime { get; set; }

    /// <summary>
    /// Gets or sets the nickname to display.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the number of failed access attempts.
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Gets or sets the lockout end time.
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets the source of the user account.
    /// </summary>
    /// <value>
    /// <para>0 - Initial</para>
    /// <para>1 - Created by administrator</para>
    /// <para>2 - User self-registration</para>
    /// </value>
    public int Source { get; set; }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the update time.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the deletion time.
    /// </summary>
    public DateTime? DeleteTime { get; set; }

    #endregion

    #region Associates

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    public HashSet<UserRole> Roles { get; set; } = [];

    /// <summary>
    /// Gets or sets the third-party authentication authorities linked to the user.
    /// </summary>
    public HashSet<UserAuthority> Authorities { get; set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Creates a new user aggregate.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static User Create(string username, int source)
    {
        var aggregate = new User(username)
        {
            Source = source
        };

        aggregate.RaiseEvent(new UserCreatedEvent(aggregate.Username) { Source = source });
        return aggregate;
    }

    /// <summary>
    /// Sets the password for the user.
    /// </summary>
    /// <param name="password">The new password in plain text.</param>
    /// <param name="changeType">The password change type. value:reset|update</param>
    internal void SetPassword(string password, string changeType = null)
    {
        var salt = RandomUtility.GenerateUniqueId();
        var hash = Cryptography.DES.Encrypt(password, Encoding.UTF8.GetBytes(salt));
        PasswordHash = hash;
        PasswordSalt = salt;
        if (!string.IsNullOrWhiteSpace(changeType))
        {
            PasswordChangedTime = DateTime.UtcNow;
            RaiseEvent(new UserPasswordChangedEvent(Id, changeType));
        }
    }

    /// <summary>
    /// Sets the email address.
    /// </summary>
    /// <param name="email"></param>
    internal void SetEmail(string email)
    {
        if (string.Equals(Email, email, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        RaiseEvent(new UserEmailChangedEvent(Id, Email, email));
    }

    /// <summary>
    /// Sets the phone number.
    /// </summary>
    /// <param name="phone"></param>
    internal void SetPhone(string phone)
    {
        if (string.Equals(Phone, phone, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        RaiseEvent(new UserPhoneChangedEvent(Id, Phone, phone));
    }

    /// <summary>
    /// Sets the nickname for the user.
    /// </summary>
    /// <param name="nickname">The new nickname.</param>
    internal void SetNickname(string nickname)
    {
        if (string.Equals(Nickname, nickname, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        Nickname = nickname;
    }

    /// <summary>
    /// Increments the access failed count and locks the account if necessary.
    /// </summary>
    internal void IncrementAccessFailedCount()
    {
        AccessFailedCount++;

        switch (AccessFailedCount)
        {
            case < 10:
                break;
            case 10:
                LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                RaiseEvent(new UserLockedEvent(Id, LockoutEnd!.Value));
                break;
            case > 10:
                LockoutEnd = LockoutEnd!.Value + TimeSpan.FromMinutes(5 * (AccessFailedCount - 10));
                break;
        }
    }

    /// <summary>
    /// Resets the access failed count and unlocks the account.
    /// </summary>
    internal void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
        LockoutEnd = null;

        RaiseEvent(new UserUnlockedEvent(Id));
    }

    /// <summary>
    /// Sets the roles for the user.
    /// </summary>
    /// <param name="roles"></param>
    internal void SetRoles(params string[] roles)
    {
        if (roles?.Any() != true)
        {
            return;
        }

        Roles ??= [];

        Roles.RemoveAll(t => !roles.Contains(t.Name, StringComparer.OrdinalIgnoreCase));

        foreach (var role in roles)
        {
            if (Roles.Any(t => t.Name.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            Roles.Add(UserRole.Create(role));
        }
    }

    /// <summary>
    /// Connects to identity provider.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="openId"></param>
    /// <param name="name"></param>
    internal void CreateAuthority(string provider, string openId, string name)
    {
        Authorities ??= [];

        if (Authorities.Any(t => string.Equals(t.Provider, provider) && string.Equals(t.OpenId, openId)))
        {
            return;
        }

        var authority = UserAuthority.Create(provider, openId);
        if (!string.IsNullOrWhiteSpace(name))
        {
            authority.SetName(name);
        }

        Authorities.Add(authority);
    }

    /// <summary>
    /// Removes the connection to identity provider.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="openId"></param>
    internal void RemoveAuthority(string provider, string openId)
    {
        Authorities ??= [];

        var authority = Authorities.FirstOrDefault(t => string.Equals(t.Provider, provider) && string.Equals(t.OpenId, openId));
        if (authority == null)
        {
            return;
        }

        Authorities.Remove(authority);
    }

    #endregion
}