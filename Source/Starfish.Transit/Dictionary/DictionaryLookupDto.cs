namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Dictionary Tree Data Transfer Object
/// </summary>
public class DictionaryLookupDto
{
    /// <summary>
    /// Get or set the key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Get or set the value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Get or set the children nodes
    /// </summary>
    public Dictionary<string, string> Items { get; set; } = [];
}
