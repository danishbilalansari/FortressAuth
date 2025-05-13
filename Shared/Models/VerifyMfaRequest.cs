using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class VerifyMfaRequest
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; }
}