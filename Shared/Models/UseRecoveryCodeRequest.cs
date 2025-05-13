using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class UseRecoveryCodeRequest
{
    [Required]
    [StringLength(10, MinimumLength = 10)]
    public string Code { get; set; }
}
