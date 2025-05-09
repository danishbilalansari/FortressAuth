using System.Security.Cryptography;

namespace IdentityServer.Services;

public class MfaRecoveryCodeService
{
    private const int CodeLength = 10; // 10-character recovery codes
    private const int NumberOfCodes = 10; // Generate 10 recovery codes

    public IEnumerable<string> GenerateNewCodes()
    {
        var codes = new List<string>();
        for (var i = 0; i < NumberOfCodes; i++)
        {
            codes.Add(GenerateCode());
        }
        return codes;
    }

    private static string GenerateCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[CodeLength];
        rng.GetBytes(bytes);

        // Use Base32 without confusing characters (remove 0, O, I, 1, etc.)
        const string chars = "ACDEFGHJKLMNPQRSTUVWXYZ23456789";
        var result = new char[CodeLength];
        for (var i = 0; i < CodeLength; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }

        return new string(result);
    }
}