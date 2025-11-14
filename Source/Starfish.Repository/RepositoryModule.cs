using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the repository module for the Starfish application.
/// </summary>
public class RepositoryModule : ModuleContextBase
{


	/// <inheritdoc />
	public override void AheadConfigureServices(ServiceConfigurationContext context)
	{
		Configure<UnitOfWorkOptions>(options =>
		{
			options.IsTransactional = false;
		});
	}

	/// <inheritdoc />
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddContextProvider()
				  .AddUnitOfWork();

		context.Services.AddHostedService<DataSeeder>();

		context.Services.AddDataContextFactory<PrimaryDataContext>()
						.AddDataContextFactory<SystemDataContext>()
						.AddDataContextFactory<IdentityDataContext>()
						.AddDataContextFactory<LoggingDataContext>();

		context.Services.AddScoped<IUserRepository, UserRepository>()
			   .AddScoped<ITokenRepository, TokenRepository>()
			   .AddScoped<IProjectRepository, ProjectRepository>()
			   .AddScoped<ITeamRepository, TeamRepository>()
			   .AddScoped<IDictionaryRepository, DictionaryRepository>();
	}


}