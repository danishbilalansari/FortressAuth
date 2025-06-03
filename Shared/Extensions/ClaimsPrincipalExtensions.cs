using System.Security.Claims;
using Shared.Constants;

namespace Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string GetUserEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Email);

    public static string GetFirstName(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AuthConstants.Claims.FirstName);

    public static string GetLastName(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AuthConstants.Claims.LastName);

    public static string GetFullName(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AuthConstants.Claims.FullName);

    public static bool IsAdmin(this ClaimsPrincipal principal)
        => principal.IsInRole(AuthConstants.Roles.Admin);
} 