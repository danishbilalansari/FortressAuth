using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace WebApp.Services;

public class ApiClientService : IApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiClientService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _httpClient.BaseAddress = new Uri("https://localhost:5002/api/");
    }

    private async Task SetAuthorizationHeader()
    {
        var context = _httpContextAccessor.HttpContext ?? 
            throw new InvalidOperationException("HttpContext is not available");
        
        var token = await context.GetTokenAsync("access_token") ?? 
            throw new InvalidOperationException("Access token not found");
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string> GetSecureDataAsync()
    {
        await SetAuthorizationHeader();
        var response = await _httpClient.GetAsync("secure/mfa-protected");
        return await HandleResponse(response);
    }

    public async Task<string> GetAdminDataAsync()
    {
        await SetAuthorizationHeader();
        var response = await _httpClient.GetAsync("secure/admin-data");
        return await HandleResponse(response);
    }

    private async Task<string> HandleResponse(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"API request failed: {error}");
    }

    public async Task<string> GetUserProfileAsync()
    {
        await SetAuthorizationHeader();
        var response = await _httpClient.GetAsync("user/profile");
        return await HandleResponse(response);
    }
}