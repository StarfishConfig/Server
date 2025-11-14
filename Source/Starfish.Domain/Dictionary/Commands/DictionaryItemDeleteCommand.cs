using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to delete items from a dictionary.
/// </summary>
public class DictionaryItemDeleteCommand : Command<long>
{
    private readonly List<string> _keys = [];

    public DictionaryItemDeleteCommand(long id, params string[] keys)
        : base(id)
    {
        _keys.AddRange(keys);
    }

    public DictionaryItemDeleteCommand(long id, IEnumerable<string> keys)
        : base(id)
    {
        _keys.AddRange(keys);
    }

    public long EntryId => Item1;

    /// <summary>
    /// Gets the keys of the items to be deleted.
    /// </summary>
    public IEnumerable<string> Keys => _keys;
}
