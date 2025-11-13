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
    internal void SetValues(Dictionary<string, string> values)
    {
        Items ??= [];

        foreach (var (key, value) in values)
        {
            var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));
            if (item != null)
            {
                item.Value = value;
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
    internal void SetValue(string key, string value)
    {
        Items ??= [];
        var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));
        if (item != null)
        {
            item.Value = value;
        }
        else
        {
            Items.Add(DictionaryItem.Create(key, value));
        }
    }

    /// <summary>
    /// Sets a single dictionary value from a key-value pair.
    /// </summary>
    /// <param name="kvp"></param>
    internal void SetValue(KeyValuePair<string, string> kvp)
    {
        SetValue(kvp.Key, kvp.Value);
    }

    /// <summary>
    /// Removes a dictionary value by key.
    /// </summary>
    /// <param name="key"></param>
    internal void RemoveValue(string key)
    {
        if (Items == null || Items.Count == 0)
        {
            return;
        }
        var item = Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.CurrentCultureIgnoreCase));
        if (item != null)
        {
            Items.Remove(item);
        }
    }
}
