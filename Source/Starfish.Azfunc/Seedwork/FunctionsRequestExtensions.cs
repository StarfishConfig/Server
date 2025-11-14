using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace Nerosoft.Starfish.Azfunc;

internal static class FunctionsRequestExtensions
{
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
