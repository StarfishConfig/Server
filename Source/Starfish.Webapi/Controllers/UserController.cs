using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "account")]
[Authorize]
public class UserController(IUserApplicationService service) : ControllerBase
{
    
}