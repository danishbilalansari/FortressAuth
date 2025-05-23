using Shared.Models;

namespace IdentityServer.Extensions;

public static class MfaRecoveryCodeExtensions
{
    public static bool IsExpired(this MfaRecoveryCode code)
    {
        return code.ExpiryDate <= DateTime.UtcNow;
    }

    public static string GetMaskedCode(this MfaRecoveryCode code)
    {
        return $"****-{code.CodeHash[^4..]}"; // Show last 4 characters
    }

    public static TimeSpan GetRemainingValidity(this MfaRecoveryCode code)
    {
        return code.ExpiryDate - DateTime.UtcNow;
    }
}