using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ChatZone.Shared.Security;

public static class SecurityHelper
{
    public static Tuple<string, string> GetHashedPasswordAndSalt(string password){
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 210000,
            numBytesRequested: 256 / 8));

        string saltBase64 = Convert.ToBase64String(salt);

        return new(hashed, saltBase64);
        //hashed => password
    } 
    
    public static string GetHashedPasswordWithSalt(string password, string salt) {
        byte[] saltBytes = Convert.FromBase64String(salt);

        string currentHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 210000,
            numBytesRequested: 256 / 8));

        return currentHashedPassword;
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public static string HashRefreshToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    
    public static string GenerateEmailAuthorizationToken()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        const int length = 70;
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }
        return new string(result);
    }
}