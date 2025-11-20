using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the configuration secret aggregate.
/// </summary>
public sealed class ConfigurationSecret : Entity<long>
{
	/// <summary>
	/// Default constructor for ORM.
	/// </summary>
	private ConfigurationSecret()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationSecret"/> class using the specified secret value.
	/// </summary>
	/// <param name="secret">The secret value to be stored. Cannot be null.</param>
	private ConfigurationSecret(string secret)
		: this()
	{
		Secret = secret;
	}

	/// <summary>
	/// Gets or sets the configuration identifier.
	/// </summary>
	public long ConfigurationId { get; set; }

	/// <summary>
	/// Gets or sets the secret value.
	/// </summary>
	public string Secret { get; private set; }

	/// <summary>
	/// Indicates whether the secret is valid.
	/// </summary>
	public bool IsValid { get; private set; }

	/// <summary>
	/// Creates a new configuration secret.
	/// </summary>
	/// <param name="secret"></param>
	/// <returns></returns>
	internal static ConfigurationSecret Create(string secret)
	{
		return new ConfigurationSecret(secret);
	}

	/// <summary>
	/// Enables the secret.
	/// </summary>
	internal void Enable()
	{
		IsValid = true;
	}

	/// <summary>
	/// Disables the secret.
	/// </summary>
	internal void Disable()
	{
		IsValid = false;
	}
}
