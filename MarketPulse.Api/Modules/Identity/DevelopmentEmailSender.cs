
namespace MarketPulse.Api.Modules.Identity;

public class DevelopmentEmailSender : IEmailSender
{
    private readonly ILogger<DevelopmentEmailSender> _logger;

    public DevelopmentEmailSender(
        ILogger<DevelopmentEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendVerificationEmailAsync(
        string email,
        string verificationToken)
    {
        _logger.LogInformation(
            "Development email verification token generated for {Email}. Token: {Token}",
            email,
            verificationToken);

        return Task.CompletedTask;
    }
}
