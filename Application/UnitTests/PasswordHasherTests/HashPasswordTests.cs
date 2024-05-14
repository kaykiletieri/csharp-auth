using CSharpAuth.Application.Services;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PasswordHasherTests;

public class HashPasswordTests
{
    private readonly PasswordHasherService _passwordHasherService;

    public HashPasswordTests()
    {
        _passwordHasherService = new PasswordHasherService();
    }

    [Fact]
    public void HashPassword_ReturnsNonNullHashAndSalt()
    {
        // Arrange
        string password = "password123";

        // Act
        var (hashedPassword, salt) = _passwordHasherService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotNull(salt);
    }

    [Fact]
    public void HashPassword_ReturnsDifferentHashesForSamePassword()
    {
        // Arrange
        string password = "password123";

        // Act
        var (hashedPassword1, _) = _passwordHasherService.HashPassword(password);
        var (hashedPassword2, _) = _passwordHasherService.HashPassword(password);

        // Assert
        Assert.NotEqual(hashedPassword1, hashedPassword2);
    }
}
