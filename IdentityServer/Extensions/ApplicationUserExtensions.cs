using IdentityServer.Data;
using Shared.Models;

namespace IdentityServer.Extension;

public static class ApplicationUserExtensions
{
    public static string GetDisplayName(this ApplicationUser user)
    {
        return $"{user.FirstName} {user.LastName}".Trim();
    }

    public static bool HasValidRecoveryCodes(this ApplicationUser user, ApplicationDbContext context)
    {
        return context.RecoveryCodes
            .Any(rc => rc.UserId == user.Id &&
                  !rc.IsUsed &&
                  rc.ExpiryDate > DateTime.UtcNow);
    }

    public static bool RequiresMfaSetup(this ApplicationUser user)
    {
        return !user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecret);
    }
}