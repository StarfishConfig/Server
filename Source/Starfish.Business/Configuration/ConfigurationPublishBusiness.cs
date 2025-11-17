using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business object for publishing a configuration.
/// </summary>
/// <param name="repository"></param>
internal class ConfigurationPublishBusiness(IConfigurationRepository repository)
	: CommandObjectBase<ConfigurationPublishBusiness>
{
	private Configuration Aggregate { get; set; }

	protected override void AddRules()
	{
		Rules.AddRule(new VersionCheckRule(repository));
	}

	#region Properties

	public static readonly PropertyInfo<string> VersionProperty = RegisterProperty<string>(p => p.Version);

	public string Version
	{
		get => ReadProperty(VersionProperty);
		set
		{
			var fieldData = FieldManager.GetFieldData(VersionProperty);
			var oldValue = fieldData switch
			{
				null => VersionProperty.DefaultValue,
				IFieldData<string> fd => fd.Value,
				_ => (string)fieldData.Value
			};

			if (string.Equals(oldValue, value))
			{
				return;
			}

			LoadPropertyValue(VersionProperty, oldValue, value, true);
		}
	}

	public static readonly PropertyInfo<string> CommentProperty = RegisterProperty<string>(p => p.Comment);

	public string Comment
	{
		get => ReadProperty(CommentProperty);
		set => LoadProperty(CommentProperty, value);
	}

	public static readonly PropertyInfo<string> OperatorProperty = RegisterProperty<string>(p => p.Operator);

	public string Operator
	{
		get => ReadProperty(OperatorProperty);
		set => LoadProperty(OperatorProperty, value);
	}

	#endregion

	[FactoryFetch]
	private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		Aggregate = await repository.GetAsync(id, true, [], cancellationToken);
	}

	[FactoryExecute]
	protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		Aggregate.Publish(Version, Comment, Operator);
		await repository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	/// <summary>
	/// Rule to check the version number before publishing.
	/// </summary>
	private class VersionCheckRule : RuleBase
	{
		private readonly IConfigurationRepository _repository;

		public VersionCheckRule(IConfigurationRepository repository)
		{
			_repository = repository;
		}

		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			if (context.Target is not ConfigurationPublishBusiness target)
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(target.Version))
			{
				context.AddErrorResult("Version number cannot be empty.");
			}
			else if (!System.Version.TryParse(target.Version, out _))
			{
				context.AddErrorResult("Invalid version number format.");
			}
			else
			{
				var exists = await _repository.ExistsVersionAsync(target.Aggregate.Id, target.Version, cancellationToken);
				if (exists)
				{
					context.AddErrorResult(string.Format(Resources.IDS_ERROR_CONFIG_VERSION_NUMBER_EXISTS, target.Version));
				}
			}
		}
	}
}