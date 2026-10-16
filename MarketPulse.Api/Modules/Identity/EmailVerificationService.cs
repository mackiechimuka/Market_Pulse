using System.Security.Cryptography;
using System.Text;
using MarketPulse.Api.Modules.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Identity;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly MarketPulseDbContext _dbContext;

    private readonly IEmailSender _emailSender;

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(24);

    public EmailVerificationService(MarketPulseDbContext dbContext, IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }

    public async Task<string> CreateVerificationTokenAsync(User user)
    {
        var existingUser = await _dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == user.Id);

        if (existingUser is null)
        {
            throw new InvalidOperationException("User was not found.");
        }

        if (existingUser.EmailVerified)
        {
            throw new InvalidOperationException("Email is already verified.");
        }

        var activeTokens = await _dbContext.EmailVerificationTokens
            .Where(x => x.UserId == existingUser.Id && x.UsedAtUtc == null)
            .ToListAsync();

        foreach (var activeToken in activeTokens)
        {
            activeToken.UsedAtUtc = DateTime.UtcNow;
        }

        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToBase64String(tokenBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        var tokenHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        var verificationToken = new EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            UserId = existingUser.Id,
            TokenHash = tokenHash,
            ExpiresAtUtc = DateTime.UtcNow.Add(TokenLifetime),
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.EmailVerificationTokens.Add(verificationToken);
        await _dbContext.SaveChangesAsync();
        await _emailSender.SendVerificationEmailAsync(
             existingUser.Email,
         token);

        return token;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var tokenHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        var verificationToken = await _dbContext.EmailVerificationTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == tokenHash);

        if (verificationToken is null ||
            verificationToken.UsedAtUtc.HasValue ||
            verificationToken.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return false;
        }

        verificationToken.User.EmailVerified = true;
        verificationToken.UsedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
