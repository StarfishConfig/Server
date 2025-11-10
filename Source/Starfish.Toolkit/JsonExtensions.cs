using System.Text.Json;
using System.Text.Json.Nodes;

namespace Nerosoft.Starfish.Toolkit;

/// <summary>
/// JSON extension methods.
/// </summary>

public static class JsonExtensions
{
    /// <summary>
    /// Gets the value of a property from a JsonNode using a dot-separated or slash-separated path.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="node"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TValue GetValue<TValue>(this JsonNode node, string name)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node), "JsonNode cannot be null.");
        }
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        var names = name.Split('.', '/');
        return GetValue<TValue>(node, names);
    }

    /// <summary>
    /// Gets the value of a property from a JsonNode using a sequence of names or indices.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="node"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TValue GetValue<TValue>(this JsonNode node, params object[] names)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node), "JsonNode cannot be null.");
        }
        if (names is null || names.Length == 0)
        {
            throw new ArgumentException("At least one name must be provided.", nameof(names));
        }

        JsonNode currentNode = node;
        foreach (var name in names)
        {
            if (currentNode is JsonObject jsonObject && jsonObject.TryGetPropertyValue(name.ToString(), out var nextNode))
            {
                currentNode = nextNode;
            }
            else if (currentNode is JsonArray jsonArray && int.TryParse(name.ToString(), out int index) && index >= 0 && index < jsonArray.Count)
            {
                currentNode = jsonArray[index];
            }
            else
            {
                return default; // Return default value if the path does not exist
            }
        }

        return currentNode switch
        {
            JsonValue jsonValue => jsonValue.GetValue<TValue>(),
            JsonObject jsonObject => jsonObject.Deserialize<TValue>(),
            _ => default // Return default value if the final node is not a JsonValue or JsonObject
        };
    }
}
