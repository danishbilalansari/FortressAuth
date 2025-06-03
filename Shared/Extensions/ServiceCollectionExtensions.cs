using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using Shared.Constants;

namespace Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IdentityBuilder AddSharedIdentity<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        return services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<TContext>()
        .AddDefaultTokenProviders();
    }

    public static IServiceCollection AddSharedAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthConstants.Policies.RequireAdmin, policy =>
                policy.RequireRole(AuthConstants.Roles.Admin));

            options.AddPolicy(AuthConstants.Policies.RequireMfa, policy =>
                policy.RequireClaim("amr", "mfa"));
        });

        return services;
    }
} 