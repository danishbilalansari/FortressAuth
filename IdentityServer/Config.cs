using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Shared.Constants;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(AuthConstants.Scopes.Api, "FortressAuth API")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource(AuthConstants.Scopes.Api, "FortressAuth API")
            {
                Scopes = { AuthConstants.Scopes.Api }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // Web Application Client
            new Client
            {
                ClientId = "webapp",
                ClientName = "Web Application",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = true,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { "https://localhost:7272/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7272/signout-callback-oidc" },
                AllowedScopes =
                {
                    AuthConstants.Scopes.OpenId,
                    AuthConstants.Scopes.Profile,
                    AuthConstants.Scopes.Email,
                    AuthConstants.Scopes.Api
                },
                RequireConsent = false,
                AllowOfflineAccess = true
            }
        };
}