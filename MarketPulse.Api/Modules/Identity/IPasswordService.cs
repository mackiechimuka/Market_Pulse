namespace MarketPulse.Api.Modules.Identity;

public interface IPasswordService
{
    string HashPassword(User user, string password);

    bool VerifyPassword(User user, string password);
}