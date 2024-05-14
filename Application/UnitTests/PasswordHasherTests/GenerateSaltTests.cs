using CSharpAuth.Application.Services;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PasswordHasherTests;

public class GenerateSaltTests
{
    private readonly PasswordHasherService _passwordHasherService;

    public GenerateSaltTests()
    {
        _passwordHasherService = new PasswordHasherService();
    }

    [Fact]
    public void GenerateSalt_ReturnsNonNullSalt()
    {
        // Act
        byte[] salt = _passwordHasherService.GenerateSalt();

        // Assert
        Assert.NotNull(salt);
    }

    [Fact]
    public void GenerateSalt_ReturnsDifferentSalts()
    {
        // Act
        byte[] salt1 = _passwordHasherService.GenerateSalt();
        byte[] salt2 = _passwordHasherService.GenerateSalt();

        // Assert
        Assert.NotEqual(salt1, salt2);
    }

    [Fact]
    public void GenerateSalt_ReturnsSaltOfCorrectLength()
    {
        // Act
        byte[] salt = _passwordHasherService.GenerateSalt();

        // Assert
        Assert.Equal(16, salt.Length);
    }
}
