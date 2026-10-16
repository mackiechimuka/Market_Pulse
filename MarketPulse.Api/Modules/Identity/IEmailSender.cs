
namespace MarketPulse.Api.Modules.Identity;

public interface IEmailSender
{
    Task SendVerificationEmailAsync(
        string email,
        string verificationToken);
}
