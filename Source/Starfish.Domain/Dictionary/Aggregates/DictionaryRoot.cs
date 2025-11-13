using System.Data;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the root entity for a dictionary.
/// </summary>
public sealed class DictionaryRoot : Aggregate<long>, IAuditing
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private DictionaryRoot()
    {
        Register<DictionaryCodeChangedEvent>(@event =>
        {
            Code = @event.NewValue;
        });
        Register<DictionaryNameChangedEvent>(@event =>
        {
            Name = @event.NewValue;
        });
    }

    /// <summary>
    /// Constructor with code and name.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    private DictionaryRoot(string code, string name)
        : this()
    {
        Code = code;
        Name = name;
    }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the remark text.
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="DictionaryRoot"/> is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the <see cref="DictionaryRoot"/> was created.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the <see cref="DictionaryRoot"/> was last updated.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <inheritdoc />
    public string CreatedBy { get; set; }

    /// <inheritdoc />
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the collection of dictionary values associated with this dictionary root.
    /// </summary>
    public HashSet<DictionaryItem> Items { get; set; }

    /// <summary>
    /// Creates a new <see cref="DictionaryRoot"/> instance with the specified code and name.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static DictionaryRoot Create(string code, string name)
    {
        return new DictionaryRoot(code, name);
    }

    /// <summary>
    /// Sets the code of the dictionary root.
    /// </summary>
    /// <param name="code"></param>
    internal void SetCode(string code)
    {
        if (string.Equals(Code, code))
        {
            return;
        }

        RaiseEvent(new DictionaryCodeChangedEvent(Id, Code, code));
    }

    /// <summary>
    /// Sets the name of the dictionary root.
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        if (string.Equals(Name, name))
        {
            return;
        }

        RaiseEvent(new DictionaryNameChangedEvent(Id, Name, name));
    }

    /// <summary>
    /// Sets the remark of the dictionary root.
    /// </summary>
    /// <param name="remark"></param>
    internal void SetRemark(string remark)
    {
        Remark = remark;
    }

    /// <summary>
    /// Sets the dictionary values.
    /// </summary>
    /// <param name="values"></param>
    internal void SetItem(Dictionary<string, string> values)
    {
        Items ??= [];

        foreach (var (key, value) in values)
        {
            var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));
            if (item != null)
            {
                item.SetKey(key);
                item.SetValue(value);
            }
            else
            {
                Items.Add(DictionaryItem.Create(key, value));
            }
        }

        // Remove values that are not in the new set
        Items.RemoveWhere(x => !values.ContainsKey(x.Key));
    }

    /// <summary>
    /// Sets a single dictionary value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="remark"></param>
    internal void SetItem(string key, string value, string remark)
    {
        Items ??= [];
        var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));

        if (item == null)
        {
            throw new NotFoundException();
        }

        item.SetKey(key);
        item.SetValue(value);
        item.SetRemark(remark);
    }

    /// <summary>
    /// Adds a single dictionary value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="remark"></param>
    /// <exception cref="DuplicateNameException"></exception>
    internal void AddItem(string key, string value, string remark = null)
    {
        Items ??= [];
        var exists = Items.Any(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));
        if (exists)
        {
            throw new DuplicateNameException($"An item with the key '{key}' already exists.");
        }

        var item = DictionaryItem.Create(key, value);
        item.SetRemark(remark);
        Items.Add(item);
    }

    /// <summary>
    /// Sets a single dictionary value from a key-value pair.
    /// </summary>
    /// <param name="kvp"></param>
    internal void SetItem(KeyValuePair<string, string> kvp)
    {
        SetItem(kvp.Key, kvp.Value, null);
    }

    /// <summary>
    /// Removes a dictionary value by key.
    /// </summary>
    /// <param name="key"></param>
    internal void RemoveItem(string key)
    {
        if (Items == null || Items.Count == 0)
        {
            return;
        }

        var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));

        if (item == null)
        {
            throw new NotFoundException();
        }

        Items.Remove(item);
    }
}