namespace WebApp.Models;

public class ProfileViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool MfaEnabled { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public ProfileViewModel()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }

    public ProfileViewModel(string firstName, string lastName, string email, bool mfaEnabled = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        MfaEnabled = mfaEnabled;
    }
}