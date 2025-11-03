using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the project aggregate.
/// </summary>
public sealed class Project : Aggregate<long>
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private Project()
    {
        Register<ProjectNameChangedEvent>(@event =>
        {
            Name = @event.NewValue;
        });
    }

    private Project(long teamId, string name)
        : this()
    {
        TeamId = teamId;
        Name = name;
    }

    /// <summary>
    /// Gets or sets the team ID associated with the project.
    /// </summary>
    public long TeamId { get; set; }

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

    /// <summary>
    /// Creates a new project instance.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static Project Create(long teamId, string name)
    {
        var project = new Project(teamId, name);
        project.RaiseEvent(new ProjectCreatedEvent(teamId, name));
        return project;
    }

    /// <summary>
    /// Sets the project name.
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        Check.EnsureNotNullOrWhiteSpace(name, nameof(name));

        if (string.Equals(Name, name, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        RaiseEvent(new ProjectNameChangedEvent(Id, Name, name));
    }

    /// <summary>
    /// Sets the project URL.
    /// </summary>
    /// <param name="url"></param>
    internal void SetUrl(string url)
    {
        if (string.Equals(Url, url, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        Url = url;
    }

    /// <summary>
    /// Sets the project description.
    /// </summary>
    /// <param name="description"></param>
    internal void SetDescription(string description)
    {
        if (string.Equals(Description, description, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        Description = description;
    }

    /// <summary>
    /// Sets the project image.
    /// </summary>
    /// <param name="image"></param>
    internal void SetImage(string image)
    {
        Check.EnsureNotNullOrWhiteSpace(image, nameof(image));

        if (string.Equals(Image, image, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        Image = image;
    }

    /// <summary>
    /// Removes the project image.
    /// </summary>
    internal void RemoveImage()
    {
        Image = null;
    }
}