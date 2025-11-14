using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// Extensions for Swagger integration.
/// </summary>
internal static class SwaggerExtensions
{
	private static readonly Dictionary<string, string> _apiGroups = new()
	{
		[ApiGroupConstants.CoreBusiness] = "Core business Service",
		[ApiGroupConstants.Identity] = "Identity Service",
		[ApiGroupConstants.Logging] = "Logging service",
		[ApiGroupConstants.System] = "System service",
	};

	/// <summary>
	/// Adds the Swagger services.
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer()
		        .AddSwaggerGen(gen =>
		        {
			        gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			        {
				        Name = "Authorization",
				        Type = SecuritySchemeType.Http,
				        Scheme = "bearer"
			        });

			        gen.AddSecurityRequirement(new OpenApiSecurityRequirement
			        {
				        {
					        new OpenApiSecurityScheme
					        {
						        Reference = new OpenApiReference
						        {
							        Type = ReferenceType.SecurityScheme,
							        Id = "Bearer"
						        }
					        },
					        new List<string>()
				        }
			        });

			        foreach (var (key, value) in _apiGroups)
			        {
				        gen.SwaggerDoc(key, new OpenApiInfo
				        {
					        Title = key,
					        Version = "v1",
					        Description = value,
					        License = new OpenApiLicense
					        {
						        Name = $"© {DateTime.Today.Year} Nerosoft. All Rights Reserved."
					        }
				        });
			        }

			        foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
			        {
				        gen.IncludeXmlComments(file);
			        }

			        gen.DocInclusionPredicate((doc, description) =>
			        {
				        return description.GroupName == null || description.GroupName.Equals(doc, StringComparison.OrdinalIgnoreCase);
			        });
		        });
	}

	/// <summary>
	/// Adds the Swagger middleware.
	/// </summary>
	/// <param name="app"></param>
	public static void UseSwagger(this IApplicationBuilder app)
	{
		// ReSharper disable once UnusedLambdaParameter
		app.UseSwagger(options =>
		{
			options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
			//option.SerializeAsV2 = true;
		});
		app.UseSwaggerUI(option =>
		{
			option.CacheLifetime = TimeSpan.FromMinutes(1);
			option.DocumentTitle = "Starfish Webapi Documentation";
			foreach (var (key, name) in _apiGroups)
			{
				option.SwaggerEndpoint($"/swagger/{key}/swagger.json", name);
			}
		});
	}
}