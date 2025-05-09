using System.Security.Cryptography;

namespace IdentityServer.Services;

public class MfaRecoveryCodeService
{
    private const int CodeLength = 10; // 10-digit recovery codes
    private const int NumberOfCodes = 10; // Generate 10 codes

    public IEnumerable<string> GenerateNewCodes()
    {
        var codes = new List<string>();

        using (var rng = RandomNumberGenerator.Create())
        {
            for (int i = 0; i < NumberOfCodes; i++)
            {
                var code = GenerateCode(rng);
                codes.Add(code);
            }
        }

        return codes;
    }

    private string GenerateCode(RandomNumberGenerator rng)
    {
        // Exclude confusing characters: 0, O, I, 1, etc.
        const string chars = "ACDEFGHJKLMNPQRSTUVWXYZ23456789";
        var buffer = new byte[CodeLength];
        rng.GetBytes(buffer);

        var result = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
        {
            result[i] = chars[buffer[i] % chars.Length];
        }

        return new string(result);
    }
}