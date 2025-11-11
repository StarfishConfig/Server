using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing team-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Project)]
public partial class TeamController : ControllerBase
{
}