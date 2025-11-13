using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Represents a value in a dictionary.
/// </summary>
public class DictionaryValue : Entity<long>
{
    private DictionaryValue()
    { }

    private DictionaryValue(string key, string value)
        : this()
    {
        Key = key;
        Value = value;
    }

    public long RootId { get; set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the remark.
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="DictionaryValue"/> from a key and value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static DictionaryValue Create(string key, string value)
    {
        return new DictionaryValue(key, value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="DictionaryValue"/> from a key-value pair.
    /// </summary>
    /// <param name="kvp"></param>
    /// <returns></returns>
    internal static DictionaryValue Create(KeyValuePair<string, string> kvp)
    {
        return Create(kvp.Key, kvp.Value);
    }
}