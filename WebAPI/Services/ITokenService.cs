using Shared.Models;

namespace WebAPI.Services;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user);
}