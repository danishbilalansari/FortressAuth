namespace Shared.Models;

public class MfaRecoveryCode
{
    // Properties (unchanged)
    public int Id { get; set; }
    public string CodeHash { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddMonths(3);
    public string UserId { get; set; }

    // Extension methods moved here
    public bool IsExpired() => ExpiryDate <= DateTime.UtcNow;

    public void MarkAsUsed()
    {
        IsUsed = true;
        UsedDate = DateTime.UtcNow;
    }
}