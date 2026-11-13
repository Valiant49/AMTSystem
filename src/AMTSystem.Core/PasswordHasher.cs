using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AMTSystem.Core;

public static class PasswordHasher
{
    private const int SaltSize = 16;      // 128-bit
    private const int KeySize = 32;       // 256-bit
    private const int Iterations = 600_000; // OWASP guidance for PBKDF2-HMAC-SHA256
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    // Output format: iterations.salt.hash (all base64 where applicable)
    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 3) return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] expected = Convert.FromBase64String(parts[2]);

        byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expected.Length);

        // Constant-time comparison to avoid timing attacks
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
