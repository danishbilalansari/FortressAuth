namespace Shared.Constants;

public static class AuthConstants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class Claims
    {
        public const string FirstName = "given_name";
        public const string LastName = "family_name";
        public const string FullName = "name";
        public const string Role = "role";
    }

    public static class Policies
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireMfa = "RequireMfa";
    }

    public static class Scopes
    {
        public const string OpenId = "openid";
        public const string Profile = "profile";
        public const string Email = "email";
        public const string Api = "api1";
    }
} 