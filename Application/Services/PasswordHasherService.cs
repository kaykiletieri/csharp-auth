using CSharpAuth.Application.Services.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace CSharpAuth.Application.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public (string HashedPassword, byte[] Salt) HashPassword(string password)
    {
        var salt = GenerateSalt();

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 65536,
            Iterations = 4
        };

        byte[] hash = argon2.GetBytes(16);
        string hashedPassword = Convert.ToBase64String(hash);


        return (hashedPassword, salt);
    }

    public bool VerifyPassword(string password, string hashedPassword, byte[] salt)
    {
        byte[] hashToVerify = Convert.FromBase64String(hashedPassword);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 65536,
            Iterations = 4
        };

        byte[] computedHash = argon2.GetBytes(hashToVerify.Length);

        bool isEqual = ConstantTimeEquals(computedHash, hashToVerify);
        return isEqual;
    }

    private static bool ConstantTimeEquals(byte[] a, byte[] b)
    {
        if (a == null || b == null || a.Length != b.Length)
            return false;

        int result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }
        return result == 0;
    }
    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}