using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the command to update a project.
/// </summary>
/// <param name="id"></param>
public sealed class ProjectUpdateCommand(long id)
	: Command<long>(id)
{
	/// <summary>
	/// Gets the entry ID.
	/// </summary>
	public long EntryId => Item1;

	/// <summary>
	/// Gets or sets the project name.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the project description.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Gets or sets the project URL.
	/// </summary>
	public string Url { get; set; }

	/// <summary>
	/// Gets or sets the project image.
	/// </summary>
	public string Image { get; set; }
}