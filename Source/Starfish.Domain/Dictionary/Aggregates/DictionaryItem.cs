using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Represents a value in a dictionary.
/// </summary>
public class DictionaryItem : Entity<long>
{
    private DictionaryItem()
    { }

    private DictionaryItem(string key, string value)
        : this()
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the ID of the dictionary root this item belongs to.
    /// </summary>
    public long RootId { get; private set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Gets or sets the remark.
    /// </summary>
    public string Remark { get; private set; }

    /// <summary>
    /// Gets or sets the dictionary root this item belongs to.
    /// </summary>
    public DictionaryRoot Root { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="DictionaryItem"/> from a key and value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static DictionaryItem Create(string key, string value)
    {
        return new DictionaryItem(key, value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="DictionaryItem"/> from a key-value pair.
    /// </summary>
    /// <param name="kvp"></param>
    /// <returns></returns>
    internal static DictionaryItem Create(KeyValuePair<string, string> kvp)
    {
        return Create(kvp.Key, kvp.Value);
    }

    internal void SetKey(string key)
    {
        if (string.Equals(Key, key))
        {
            return;
        }
        Key = key;
    }

    internal void SetValue(string value)
    {
        if (string.Equals(Value, value))
        {
            return;
        }
        Value = value;
    }

    /// <summary>
    /// Sets the remark of the dictionary item.
    /// </summary>
    /// <param name="remark"></param>
    internal void SetRemark(string remark)
    {
        Remark = remark;
    }
}