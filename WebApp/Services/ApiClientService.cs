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
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = new Uri("https://localhost:5002/api/");
    }

    public async Task<string> GetSecureDataAsync()
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("secure/mfa-protected");
        return await HandleResponse(response);
    }

    public async Task<string> GetAdminDataAsync()
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("secure/admin-data");
        return await HandleResponse(response);
    }

    private async Task<string> HandleResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"API request failed: {error}");
    }

    public async Task<string> GetUserProfileAsync()
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("user/profile");
        return await HandleResponse(response);
    }
}