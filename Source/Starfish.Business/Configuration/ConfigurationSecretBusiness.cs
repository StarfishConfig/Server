using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

public class ConfigurationSecretBusiness(IConfigurationRepository repository) : EditableObjectBase<ConfigurationSecretBusiness, Configuration>
{
	private Configuration _aggregate;
	protected override Configuration Aggregate => _aggregate;

	#region Properties

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);

	public long Id
	{
		get => GetProperty(IdProperty);
		set => SetProperty(IdProperty, value);
	}

	public static readonly PropertyInfo<string> SecretProperty = RegisterProperty<string>(p => p.Secret);

	public string Secret
	{
		get => GetProperty(SecretProperty);
		set => SetProperty(SecretProperty, value);
	}

	#endregion

	#region Methods

	[FactoryFetch]
	private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		_aggregate = await repository.GetAsync(id, true, [], cancellationToken: cancellationToken);
		LoadProperty(IdProperty, _aggregate.Id);
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		Aggregate.SetSecret(Secret);
		await repository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	#endregion
}