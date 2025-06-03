using Microsoft.AspNetCore.Identity;

namespace Shared.Models;

public class ApplicationUser : IdentityUser
{
    // Core MFA properties
    public string MfaSecret { get; set; }
    public bool MfaEnabled { get; set; }
    public DateTime MfaEnabledDate { get; set; }

    // Security timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastPasswordChangeDate { get; set; }

    // Relationships (EF Core will handle this)
    public virtual ICollection<MfaRecoveryCode> RecoveryCodes { get; set; } = new List<MfaRecoveryCode>();

    // Profile fields
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Extension methods moved here
    public string GetFullName() => $"{FirstName} {LastName}";

    public bool HasValidRecoveryCodes() => RecoveryCodes.Any(rc => !rc.IsUsed && rc.ExpiryDate > DateTime.UtcNow);

    public bool IsMfaEnabled { get; set; }
    public string PreferredMfaMethod { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;
}