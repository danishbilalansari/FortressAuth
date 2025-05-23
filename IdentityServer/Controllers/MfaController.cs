using IdentityServer.Data;
using IdentityServer.Extension;
using IdentityServer.Models.MfaViewModels;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Require authentication for all endpoints
public class MfaController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TOTPService _totpService;
    private readonly MfaRecoveryCodeService _recoveryService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MfaController> _logger;

    public MfaController(
        UserManager<ApplicationUser> userManager,
        TOTPService totpService,
        MfaRecoveryCodeService recoveryService,
        ApplicationDbContext context,
        ILogger<MfaController> logger)
    {
        _userManager = userManager;
        _totpService = totpService;
        _recoveryService = recoveryService;
        _context = context;
        _logger = logger;
    }

    [HttpGet("setup")]
    public async Task<IActionResult> SetupMfa()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!user.RequiresMfaSetup())
            {
                return BadRequest("MFA is already configured");
            }

            // Generate or get existing secret key
            var secretKey = await _userManager.GetAuthenticatorKeyAsync(user)
                ?? await ResetAuthenticatorKey(user);

            var qrCodeUri = _totpService.GenerateQrCodeUri(
                user.Email,
                secretKey,
                "FortressAuth");

            return Ok(new MfaSetupViewModel
            {
                SecretKey = secretKey,
                QrCodeUri = qrCodeUri,
                IsMfaEnabled = user.MfaEnabled
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up MFA");
            return StatusCode(500, "An error occurred during MFA setup");
        }
    }

    [HttpPost("verify")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");

            var secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(secretKey))
                return BadRequest("No MFA secret key found");

            if (_totpService.ValidateCode(secretKey, request.Code))
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                user.MfaEnabled = true;
                await _userManager.UpdateAsync(user);

                // Generate and return recovery codes (hashed)
                var recoveryCodes = await GenerateAndStoreRecoveryCodes(user);

                return Ok(new
                {
                    Success = true,
                    RecoveryCodes = recoveryCodes
                });
            }

            return BadRequest("Invalid verification code");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying MFA code");
            return StatusCode(500, "An error occurred during MFA verification");
        }
    }

    [HttpGet("recovery-codes")]
    public async Task<IActionResult> GetRecoveryCodes()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!user.MfaEnabled)
            return BadRequest("MFA is not enabled");

        var validCodes = await _context.RecoveryCodes
            .Where(rc => rc.UserId == user.Id && !rc.IsUsed && !rc.IsExpired())
            .ToListAsync();

        return Ok(new { Count = validCodes.Count });
    }

    [HttpPost("recovery-codes/generate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateNewRecoveryCodes()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!user.MfaEnabled)
            return BadRequest("MFA is not enabled");

        var newCodes = await GenerateAndStoreRecoveryCodes(user);
        return Ok(new { RecoveryCodes = newCodes });
    }

    [HttpPost("recovery-codes/use")]
    public async Task<IActionResult> UseRecoveryCode([FromBody] UseRecoveryCodeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Find and validate the recovery code
            var recoveryCode = await _context.RecoveryCodes
                .FirstOrDefaultAsync(rc =>
                    rc.UserId == user.Id &&
                    !rc.IsUsed &&
                    !rc.IsExpired() &&
                    rc.CodeHash == HashCode(request.Code));

            if (recoveryCode == null)
                return BadRequest("Invalid or expired recovery code");

            // Mark code as used
            recoveryCode.MarkAsUsed();
            await _context.SaveChangesAsync();

            // Refresh security stamp
            await _userManager.UpdateSecurityStampAsync(user);

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error using recovery code");
            return StatusCode(500, "An error occurred");
        }
    }

    private async Task<string> ResetAuthenticatorKey(ApplicationUser user)
    {
        await _userManager.ResetAuthenticatorKeyAsync(user);
        return await _userManager.GetAuthenticatorKeyAsync(user);
    }

    private async Task<List<string>> GenerateAndStoreRecoveryCodes(ApplicationUser user)
    {
        // Generate new codes (plaintext for one-time display)
        var newCodes = _recoveryService.GenerateNewCodes().ToList();

        // Using extension via LINQ
        var expiringSoon = await _context.RecoveryCodes
            .Where(rc => rc.UserId == user.Id && rc.IsExpired())
            .ToListAsync();

        if (expiringSoon.Any())
        {
            _logger.LogWarning($"User {user.GetDisplayName()} has {expiringSoon.Count} codes expiring soon");
        }

        // Remove old codes
        var oldCodes = await _context.RecoveryCodes
            .Where(rc => rc.UserId == user.Id)
            .ToListAsync();
        _context.RecoveryCodes.RemoveRange(oldCodes);

        // Store hashed versions
        var recoveryCodes = newCodes.Select(code => new MfaRecoveryCode
        {
            CodeHash = HashCode(code),
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddMonths(3)
        }).ToList();

        await _context.RecoveryCodes.AddRangeAsync(recoveryCodes);
        await _context.SaveChangesAsync();

        return newCodes;
    }

    private string HashCode(string code)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));
        return Convert.ToBase64String(bytes);
    }
}