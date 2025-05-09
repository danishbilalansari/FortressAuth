namespace IdentityServer.Models.MfaViewModels;

public class VerifyViewModel
{
    public string Code { get; set; }
    public string ReturnUrl { get; set; }
}