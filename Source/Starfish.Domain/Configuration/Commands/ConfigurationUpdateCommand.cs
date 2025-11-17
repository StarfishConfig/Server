using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to update a configuration entry.
/// </summary>
/// <param name="id"></param>
public class ConfigurationUpdateCommand(long id)
	: Command<long>(id)
{
	/// <summary>
	/// Gets the entry ID.
	/// </summary>
	public long EntryId => Item1;

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the description.
	/// </summary>
	public string Description { get; set; }
}