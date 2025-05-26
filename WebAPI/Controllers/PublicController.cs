using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/public")]
[AllowAnonymous]
public class PublicController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPublicData()
    {
        return Ok(new { Message = "This is public data" });
    }
}