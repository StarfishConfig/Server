namespace Nerosoft.Starfish.Transit;

public abstract class ProjectBaseDto
{
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