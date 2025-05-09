using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Models.MfaViewModels;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MfaController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TOTPService _totpService;
    private readonly MfaRecoveryCodeService _recoveryService;
    private readonly ApplicationDbContext _context;

    public MfaController(
        UserManager<ApplicationUser> userManager,
        TOTPService totpService,
        MfaRecoveryCodeService recoveryService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _totpService = totpService;
        _recoveryService = recoveryService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Setup()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(secretKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var email = await _userManager.GetEmailAsync(user);
        var qrCodeUri = _totpService.GenerateQrCodeUri(email, secretKey, "FortressAuth");

        return View(new MfaSetupViewModel
        {
            SecretKey = secretKey,
            QrCodeUri = qrCodeUri
        });
    }

    [HttpPost]
    public async Task<IActionResult> VerifySetup(string code)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (_totpService.ValidateCode(secretKey, code))
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);

            // Generate and store recovery codes
            var codes = _recoveryService.GenerateNewCodes().ToList();
            await ReplaceRecoveryCodesAsync(user, codes);

            TempData["RecoveryCodes"] = codes;
            return RedirectToAction("ShowRecoveryCodes");
        }

        ModelState.AddModelError("", "Invalid verification code");
        return View("Setup");
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
        var codes = TempData["RecoveryCodes"] as List<string>;
        if (codes == null) return RedirectToAction("Setup");

        return View("RecoveryCodes", codes);
    }

    [HttpPost]
    public async Task<IActionResult> UseRecoveryCode(string code, string returnUrl)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var recoveryCode = await _context.RecoveryCodes
            .FirstOrDefaultAsync(rc => rc.UserId == user.Id &&
                                     rc.Code == code &&
                                     !rc.IsUsed);

        if (recoveryCode != null)
        {
            recoveryCode.IsUsed = true;
            _context.Update(recoveryCode);
            await _context.SaveChangesAsync();

            await _userManager.UpdateSecurityStampAsync(user);
            return Redirect(returnUrl ?? "/");
        }

        ModelState.AddModelError("", "Invalid recovery code");
        return View("Verify");
    }

    private async Task ReplaceRecoveryCodesAsync(ApplicationUser user, List<string> newCodes)
    {
        // Remove old codes
        var oldCodes = await _context.RecoveryCodes
            .Where(rc => rc.UserId == user.Id)
            .ToListAsync();
        _context.RecoveryCodes.RemoveRange(oldCodes);

        // Add new codes
        var recoveryCodes = newCodes.Select(c => new MfaRecoveryCode
        {
            Code = c,
            UserId = user.Id,
            IsUsed = false
        });
        await _context.RecoveryCodes.AddRangeAsync(recoveryCodes);

        await _context.SaveChangesAsync();
    }
}
}