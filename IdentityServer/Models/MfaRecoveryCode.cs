namespace IdentityServer.Models;

public class MfaRecoveryCode
{
    public int Id { get; set; }
    public string Code { get; set; }
    public bool IsUsed { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
