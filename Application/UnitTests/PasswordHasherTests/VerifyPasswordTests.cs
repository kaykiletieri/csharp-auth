using CSharpAuth.Application.Services;
using System.Text;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PasswordHasherTests;

public class VerifyPasswordTests
{
    private readonly PasswordHasherService _passwordHasherService;

    public VerifyPasswordTests()
    {
        _passwordHasherService = new PasswordHasherService();
    }

    [Fact]
    public void VerifyPassword_ReturnsTrueForCorrectPassword()
    {
        // Arrange
        string password = "password123";
        var (hashedPassword, salt) = _passwordHasherService.HashPassword(password);

        // Act
        bool result = _passwordHasherService.VerifyPassword(password, hashedPassword, salt);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ReturnsFalseForIncorrectPassword()
    {
        // Arrange
        string password = "password123";
        var (hashedPassword, salt) = _passwordHasherService.HashPassword(password);

        // Act
        bool result = _passwordHasherService.VerifyPassword("wrongpassword", hashedPassword, salt);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_ReturnsFalseForIncorrectSalt()
    {
        // Arrange
        string password = "password123";
        var (hashedPassword, _) = _passwordHasherService.HashPassword(password);
        byte[] incorrectSalt = Encoding.UTF8.GetBytes("WrongSalt");

        // Act
        bool result = _passwordHasherService.VerifyPassword(password, hashedPassword, incorrectSalt);

        // Assert
        Assert.False(result);
    }
}
