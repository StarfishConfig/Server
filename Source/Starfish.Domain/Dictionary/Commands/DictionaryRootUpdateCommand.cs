using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to update an existing <see cref="DictionaryRoot"/>.
/// </summary>
/// <param name="id"></param>
public class DictionaryRootUpdateCommand(long id)
    : Command<long>(id)
{
    /// <summary>
    /// Gets the aggregate root identifier.
    /// </summary>
    public long EntryId => Item1;

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
}
