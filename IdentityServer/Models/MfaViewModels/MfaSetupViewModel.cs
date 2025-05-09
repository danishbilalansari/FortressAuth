namespace IdentityServer.Models.MfaViewModels;

public class MfaSetupViewModel
{
    public string SecretKey { get; set; }
    public string QrCodeUri { get; set; }
}