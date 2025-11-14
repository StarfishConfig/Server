using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// Extension methods for FunctionContext to handle request parameters.
/// </summary>
internal static class FunctionsRequestExtensions
{
	/// <summary>
	/// Gets a query parameter from the FunctionContext's BindingData.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="context"></param>
	/// <returns></returns>
	public static T GetQueryParameter<T>(this FunctionContext context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));

		var query = context.BindingContext.BindingData["Query"];

		if (query == null)
		{
			return default;
		}

		return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(query.ToString());

		// Attempt to get the parameter value from the query string
		// return (T) query;
	}
}
