using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string MfaSecret { get; set; }
    public bool MfaEnabled { get; set; }
    public ICollection<MfaRecoveryCode> RecoveryCodes { get; set; } = new List<MfaRecoveryCode>();
}