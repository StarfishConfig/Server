using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Defines the event that occurs when the user authentication fails.
/// </summary>
internal class UserAuthFailedEvent : ApplicationEvent
{
    /// <summary>
    /// Gets or sets the auth type.
    /// </summary>
    public string AuthType { get; set; }

    /// <summary>
    /// Gets or sets the additional data.
    /// </summary>
    public Dictionary<string, string> Data { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Error { get; set; }
}
