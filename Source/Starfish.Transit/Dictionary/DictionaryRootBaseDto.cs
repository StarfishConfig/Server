namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Base data transfer object for dictionary roots.
/// </summary>
public abstract class DictionaryRootBaseDto
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
