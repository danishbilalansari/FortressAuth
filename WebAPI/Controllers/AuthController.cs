using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Username);
        ArgumentNullException.ThrowIfNull(request.Password);

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null) return Unauthorized();

        var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid) return Unauthorized();

        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User) 
            ?? throw new InvalidOperationException("User not found");

        return Ok(new
        {
            user.Id,
            user.Email,
            user.MfaEnabled,
            Roles = await _userManager.GetRolesAsync(user)
        });
    }
}

public record LoginRequest(string Username, string Password)
{
    public string Username { get; init; } = Username ?? throw new ArgumentNullException(nameof(Username));
    public string Password { get; init; } = Password ?? throw new ArgumentNullException(nameof(Password));
}