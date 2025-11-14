using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to update an existing <see cref="DictionaryItem"/>.
/// </summary>
/// <param name="id"></param>
public class DictionaryItemUpdateCommand(long id)
    : Command<long>(id)
{
    public long EntryId => Item1;

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
}