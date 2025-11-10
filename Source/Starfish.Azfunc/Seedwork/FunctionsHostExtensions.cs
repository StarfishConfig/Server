using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// Extension methods for initializing the Functions host.
/// </summary>
internal static class FunctionsHostExtensions
{
    /// <summary>
    /// Initializes the application within the Functions host.
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IHost InitializeApplication(this IHost host)
    {
        var application = host.Services.GetRequiredService<IApplicationWithServiceProvider>();
        application.Initialize(host.Services);

        var constraints = host.Services.GetRequiredService<IInlineConstraintResolver>();

        var assembly = Assembly.GetEntryAssembly();

        if (assembly != null)
        {
            var types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                if (!type.IsClass)
                {
                    continue;
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                if (methods.Length == 0)
                {
                    continue;
                }

                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<FunctionAttribute>();
                    if (attribute == null)
                    {
                        continue;
                    }

                    FunctionContainer.Instance.AddFunction(attribute.Name, method);
                }
            }
        }

        {
        }

        return host;
    }
}
