namespace CSharpAuth.Application.Services.Interfaces;

public interface IPasswordHasherService
{
    (string HashedPassword, byte[] Salt) HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword, byte[] salt);
    byte[] GenerateSalt();
}
