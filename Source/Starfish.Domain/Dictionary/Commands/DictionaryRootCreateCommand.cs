using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to create a new <see cref="DictionaryRoot"/>.
/// </summary>
public class DictionaryRootCreateCommand : Command
{
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
