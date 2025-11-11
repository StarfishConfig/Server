namespace Nerosoft.Starfish.Repository;

internal abstract class TeamSearchRequest
{
    public string Keyword { get; set; }

    public bool? Owned { get; set; }

    /// <summary>
    /// Indicates whether to filter teams that the user has joined.
    /// </summary>
    public bool? Joined { get; set; }
}
