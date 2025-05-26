namespace WebApp.Services;

public interface IApiClientService
{
    Task<string> GetSecureDataAsync();
    Task<string> GetAdminDataAsync();
    Task<string> GetUserProfileAsync();
}