namespace MarketPulse.Api.Modules.Identity;

public interface IEmailVerificationService
{
    Task<string> CreateVerificationTokenAsync(User user);

    Task<bool> VerifyEmailAsync(string token);
}
