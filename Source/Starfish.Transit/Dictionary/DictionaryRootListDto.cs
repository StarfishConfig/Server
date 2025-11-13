namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data transfer object for a list of dictionary roots.
/// </summary>
public class DictionaryRootListDto : DictionaryRootBaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the dictionary root.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the data was created.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the data was last updated.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Gets or sets the user who created the data.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the user who last updated the data.
    /// </summary>
    public string UpdatedBy { get; set; }
}
