using System.Text.RegularExpressions;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Toolkit;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the configuration aggregate.
/// </summary>
public sealed class Configuration : Aggregate<long>, IAuditing
{
	#region Ctors

	/// <summary>
	/// Default constructor for ORM.
	/// </summary>
	private Configuration()
	{
		Register<ConfigurationNameChangedEvent>(@event =>
		{
			Name = @event.NewValue;
		});
		Register<ConfigurationStatusChangedEvent>(@event =>
		{
			Status = @event.NewValue;
		});
		Register<ConfigurationSecretChangedEvent>(@event =>
		{
			Secret = @event.NewValue;
		});
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Configuration"/> class.
	/// </summary>
	/// <param name="teamId"></param>
	/// <param name="name"></param>
	/// 
	private Configuration(long teamId, string name)
		: this()
	{
		TeamId = teamId;
		Name = name;
		Status = ConfigurationStatus.Pending;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the identifier of the team associated with the <see cref="Configuration"/>.
	/// </summary>
	public long TeamId { get; set; }

	/// <summary>
	/// Gets or sets the environment of the <see cref="Configuration"/>.
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// Gets or sets the cluster of the <see cref="Configuration"/>.
	/// </summary>
	public string Cluster { get; set; }

	/// <summary>
	/// Gets or sets the name of the <see cref="Configuration"/>.
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Gets or sets the description of the <see cref="Configuration"/>.
	/// </summary>
	public string Description { get; private set; }

	/// <summary>
	/// Gets or sets the status of the <see cref="Configuration"/>.
	/// </summary>
	/// <value>See <see cref="ConfigurationStatus"/></value>
	public ConfigurationStatus Status { get; private set; }

	/// <summary>
	/// Gets or sets the version number of the <see cref="Configuration"/>.
	/// </summary>
	public string Version { get; private set; }

	/// <summary>
	/// Gets or sets the time when the <see cref="Configuration"/> was published.
	/// </summary>
	public DateTime? PublishTime { get; private set; }

	/// <summary>
	/// Gets or sets the time when the <see cref="Configuration"/> was created.
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// Gets or sets the time when the <see cref="Configuration"/> was last updated.
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the user who created the <see cref="Configuration"/>.
	/// </summary>
	public string CreatedBy { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the user who last updated the <see cref="Configuration"/>.
	/// </summary>
	public string UpdatedBy { get; set; }

	#endregion

	#region Associations

	/// <summary>
	/// Gets or sets the collection of configuration items associated with the <see cref="Configuration"/>.
	/// </summary>
	public HashSet<ConfigurationItem> Items { get; set; }

	/// <summary>
	/// Gets or sets the collection of configuration revisions associated with the <see cref="Configuration"/>.
	/// </summary>
	public HashSet<ConfigurationRevision> Revisions { get; set; }

	/// <summary>
	/// Gets or sets the archive of the <see cref="Configuration"/>.
	/// </summary>
	public ConfigurationArchive Archive { get; set; }

	/// <summary>
	/// Gets or sets the collection of configuration secrets associated with the <see cref="Configuration"/>.
	/// </summary>
	public HashSet<ConfigurationSecret> Secrets { get; set; }

	#endregion

	#region Operations

	/// <summary>
	/// Creates a new configuration.
	/// </summary>
	/// <param name="teamId"></param>
	/// <param name="projectId"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	internal static Configuration Create(long teamId, long projectId, string name)
	{
		var entity = new Configuration(teamId, name);
		entity.RaiseEvent(new ConfigurationCreatedEvent(teamId, projectId, name));
		return entity;
	}

	/// <summary>
	/// Sets the name of the configuration.
	/// </summary>
	/// <param name="name"></param>
	internal void SetName(string name)
	{
		if (string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (Id > 0)
		{
			EnsureNotDisabled();
			RaiseEvent(new ConfigurationNameChangedEvent(Id, Name, name));
		}
		else
		{
			Name = name;
		}
	}

	internal void SetDescription(string description)
	{
		Description = description;
	}

	internal void SetSecret(string secret)
	{
		EnsureNotDisabled();

		ArgumentException.ThrowIfNullOrWhiteSpace(secret);

		if (!Regex.IsMatch(secret, RegexPattern.Secret))
		{
			throw new BadRequestException(Resources.IDS_ERROR_CONFIG_SECRET_NOT_MATCHES_RULE);
		}

		var secretHash = Cryptography.SHA.Encrypt(secret);

		//if (string.Equals(Secret, secretHash, StringComparison.Ordinal))
		//{
		//	return;
		//}

		//if (Id > 0)
		//{
		//	RaiseEvent(new ConfigurationSecretChangedEvent(Id, Secret, secretHash));
		//}
		//else
		//{
		//	Secret = secretHash;
		//}
	}

	internal void Disable()
	{
		EnsureNotDisabled();

		if (Status != ConfigurationStatus.Disabled)
		{
			return;
		}

		SetStatus(ConfigurationStatus.Disabled);
		RaiseEvent(new ConfigurationDisabledEvent(Id));
	}

	internal void Enable()
	{
		if (Status != ConfigurationStatus.Disabled)
		{
			return;
		}

		SetStatus(ConfigurationStatus.Pending);
		RaiseEvent(new ConfigurationEnabledEvent(Id));
	}

	/// <summary>
	/// Publishes the configuration.
	/// </summary>
	/// <param name="version"></param>
	/// <param name="comment"></param>
	/// <param name="operator"></param>
	/// <exception cref="InvalidOperationException"></exception>
	internal void Publish(string version, string comment, string @operator)
	{
		EnsureNotDisabled();

		if (Items == null || Items.Count == 0)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_NO_ITEMS);
		}

		Version = version;
		PublishTime = DateTime.Now;
		SetStatus(ConfigurationStatus.Published);
		RaiseEvent(new ConfigurationPublishedEvent(Id, version, comment, @operator));
	}

	/// <summary>
	/// Sets the status of the configuration.
	/// </summary>
	/// <param name="status"></param>
	private void SetStatus(ConfigurationStatus status)
	{
		if (Status == status)
		{
			return;
		}

		RaiseEvent(new ConfigurationStatusChangedEvent(Id, Status, status));
	}

	/// <summary>
	/// Ensures that the configuration is not disabled.
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	private void EnsureNotDisabled()
	{
		if (Status == ConfigurationStatus.Disabled)
		{
			throw new InvalidOperationException();
		}
	}

	#endregion
}