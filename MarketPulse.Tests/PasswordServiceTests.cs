using MarketPulse.Api.Modules.Identity;

namespace MarketPulse.Tests;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService = new();

    [Fact]
    public void HashPassword_ShouldNotStorePlainTextPassword()
    {
        var user = new User();
        const string password = "SecurePassword123!";

        var hash = _passwordService.HashPassword(user, password);

        Assert.NotEqual(password, hash);
        Assert.False(string.IsNullOrWhiteSpace(hash));
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        var user = new User();
        const string password = "SecurePassword123!";

        user.PasswordHash = _passwordService.HashPassword(user, password);

        var result = _passwordService.VerifyPassword(user, password);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
    {
        var user = new User();
        const string password = "SecurePassword123!";

        user.PasswordHash = _passwordService.HashPassword(user, password);

        var result = _passwordService.VerifyPassword(
            user,
            "WrongPassword123!");

        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseWhenPasswordHashIsMissing()
    {
        var user = new User();

        var result = _passwordService.VerifyPassword(
            user,
            "SecurePassword123!");

        Assert.False(result);
    }

    [Fact]
    public void HashPassword_ShouldProduceDifferentHashesForSamePassword()
    {
        var user1 = new User();
        var user2 = new User();
        const string password = "SecurePassword123!";

        var hash1 = _passwordService.HashPassword(user1, password);
        var hash2 = _passwordService.HashPassword(user2, password);

        Assert.NotEqual(hash1, hash2);
    }
}