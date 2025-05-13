using Shared.Models;

namespace IdentityServer.Models;

public static class ApplicationUserExtensions
{
    public static bool IsExpired(this MfaRecoveryCode code)
    {
        return code.ExpiryDate <= DateTime.UtcNow;
    }

    public static void MarkAsUsed(this MfaRecoveryCode code)
    {
        code.IsUsed = true;
        code.UsedDate = DateTime.UtcNow;
    }
}