namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data transfer object for specifying criteria to filter dictionary roots.
/// </summary>
public class DictionaryRootCriteriaDto
{
    /// <summary>
    /// Gets or sets the keyword of the dictionary root.
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dictionary root is valid.
    /// </summary>
    public bool? IsValid { get; set; }
}
