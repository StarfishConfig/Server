using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

internal class TeamUpdateCommand(long id) : Command<long>(id)
{
    public long TeamId => Item1;
}
