using System.Collections.Concurrent;
using System.Reflection;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// A container for storing and retrieving functions by name.
/// </summary>
internal class FunctionContainer
{
    private readonly ConcurrentDictionary<string, MethodInfo> _functions = new();

    private static readonly Lazy<FunctionContainer> _instance = new(() => new FunctionContainer(), true);

    public static FunctionContainer Instance => _instance.Value;

    /// <summary>
    /// Adds a function with the specified name and method info.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="method"></param>
    public void AddFunction(string name, MethodInfo method)
    {
        _functions.TryAdd(name, method);
    }

    /// <summary>
    /// Gets the function by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public MethodInfo GetFunction(string name)
    {
        if (_functions.TryGetValue(name, out var method))
        {
            return method;
        }

        return null;
    }

    /// <summary>
    /// Tries to get the function by name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public bool TryGetFunction(string name, out MethodInfo method)
    {
        return _functions.TryGetValue(name, out method);
    }
}