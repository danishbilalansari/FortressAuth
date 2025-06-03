namespace WebApp.Models;

public class ProfileViewModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool MfaEnabled { get; set; }

    public ProfileViewModel()
    {
        Name = string.Empty;
        Email = string.Empty;
    }

    public ProfileViewModel(string name, string email, bool mfaEnabled = false)
    {
        Name = name;
        Email = email;
        MfaEnabled = mfaEnabled;
    }
}