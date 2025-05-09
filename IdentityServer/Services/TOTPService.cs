using System.Security.Cryptography;
using OtpNet;

namespace IdentityServer.Services;

public class TOTPService
{
    private const int Digits = 6;
    private const int Step = 30; // seconds

    public string GenerateSecretKey()
    {
        var rng = RandomNumberGenerator.Create();
        var key = new byte[20];
        rng.GetBytes(key);
        return Base32Encoding.ToString(key);
    }

    public string GenerateQrCodeUri(string email, string secretKey, string issuer)
    {
        return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}?" +
               $"secret={secretKey}&issuer={Uri.EscapeDataString(issuer)}&digits={Digits}&period={Step}";
    }

    public bool ValidateCode(string secretKey, string code)
    {
        var keyBytes = Base32Encoding.ToBytes(secretKey);
        var totp = new Totp(keyBytes, Step, mode: OtpHashMode.Sha1);
        return totp.VerifyTotp(code, out _, window: new VerificationWindow(1, 1));
    }
}