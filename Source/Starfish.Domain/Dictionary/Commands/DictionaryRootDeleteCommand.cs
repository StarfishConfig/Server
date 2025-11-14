using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to delete an existing <see cref="DictionaryRoot"/>.
/// </summary>
/// <param name="id"></param>
public class DictionaryRootDeleteCommand(long id)
    : Command<long>(id)
{
    /// <summary>
    /// Gets the aggregate root identifier.
    /// </summary>
    public long EntryId => Item1;
}
