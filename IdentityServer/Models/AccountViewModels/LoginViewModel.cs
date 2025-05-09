namespace IdentityServer.Models.AccountViewModels;

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
    public bool RequiresMfa { get; set; }
}
