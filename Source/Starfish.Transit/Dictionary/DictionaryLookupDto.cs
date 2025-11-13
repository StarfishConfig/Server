namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Dictionary Tree Data Transfer Object
/// </summary>
public class DictionaryLookupDto
{
    /// <summary>
    /// Get or set the code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Get or set the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Get or set the children nodes
    /// </summary>
    public Dictionary<string, string> Items { get; set; } = [];
}
