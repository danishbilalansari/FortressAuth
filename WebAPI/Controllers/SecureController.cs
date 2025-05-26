namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/secure")]
[Authorize]
public class SecureController : ControllerBase
{
    [HttpGet("mfa-protected")]
    [Authorize(Policy = "RequireMFA")]
    public IActionResult GetMfaProtectedData()
    {
        return Ok(new
        {
            Message = "This endpoint requires MFA authentication",
            User = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet("admin-data")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdminData()
    {
        return Ok(new
        {
            Message = "Admin-only data",
            User = User.Identity?.Name
        });
    }

    [HttpPost("write-data")]
    [Authorize(Policy = "WriteAccess")]
    public IActionResult WriteData([FromBody] object data)
    {
        return Ok(new
        {
            Message = "Data written successfully",
            Data = data,
            User = User.Identity?.Name
        });
    }
}