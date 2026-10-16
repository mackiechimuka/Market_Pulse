
using MarketPulse.Api.Modules.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Identity;

[ApiController]
[Route("api/identity/email-verification")]
public class EmailVerificationController : ControllerBase
{
    private readonly MarketPulseDbContext _dbContext;
    private readonly IEmailVerificationService _emailVerificationService;

    public EmailVerificationController(
        MarketPulseDbContext dbContext,
        IEmailVerificationService emailVerificationService)
    {
        _dbContext = dbContext;
        _emailVerificationService = emailVerificationService;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestVerification(
        [FromBody] RequestVerificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new
            {
                message = "A valid email address is required."
            });
        }

        var normalizedEmail = request.Email.Trim();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail);

        // Return the same response whether the account exists,
        // is already verified, or does not exist.
        if (user is not null && !user.EmailVerified)
        {
            await _emailVerificationService
                .CreateVerificationTokenAsync(user);
        }

        return Ok(new
        {
            message = "If an eligible account exists, a verification email will be sent."
        });
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmVerification(
        [FromBody] ConfirmVerificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new
            {
                message = "A verification token is required."
            });
        }

        var verified = await _emailVerificationService
            .VerifyEmailAsync(request.Token);

        if (!verified)
        {
            return BadRequest(new
            {
                message = "The verification token is invalid, expired, or already used."
            });
        }

        return Ok(new
        {
            message = "Email verified successfully."
        });
    }
}

public sealed record RequestVerificationRequest(string Email);

public sealed record ConfirmVerificationRequest(string Token);
