using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the command to delete a project.
/// </summary>
/// <param name="id"></param>
public sealed class ProjectDeleteCommand(long id)
	: Command<long>(id)
{
	/// <summary>
	/// Gets the entry ID.
	/// </summary>
	public long EntryId => Item1;
}