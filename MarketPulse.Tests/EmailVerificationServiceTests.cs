
using MarketPulse.Api.Modules.Identity;
using MarketPulse.Api.Modules.Tenancy;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Tests;

public class EmailVerificationServiceTests
{
    private static async Task<(SqliteConnection Connection, MarketPulseDbContext DbContext)>
       
    CreateDatabaseAsync()
{
    var connection = new SqliteConnection("Data Source=:memory:");
    await connection.OpenAsync();

    var options = new DbContextOptionsBuilder<MarketPulseDbContext>()
        .UseSqlite(connection)
        .Options;

    ITenantContext tenantContext = new TenantContext();

    var dbContext = new MarketPulseDbContext(options, tenantContext);
    await dbContext.Database.EnsureCreatedAsync();

    return (connection, dbContext);
}


    [Fact]
    public async Task CreateVerificationTokenAsync_ShouldCreateTokenForUnverifiedUser()
    {
        var (connection, dbContext) = await CreateDatabaseAsync();

        try
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                EmailVerified = false
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var service = new EmailVerificationService(dbContext, new FakeEmailSender());

            var token = await service.CreateVerificationTokenAsync(user);

            Assert.False(string.IsNullOrWhiteSpace(token));

            var savedToken = await dbContext.EmailVerificationTokens
                .SingleAsync(x => x.UserId == user.Id);

            Assert.NotEqual(token, savedToken.TokenHash);
            Assert.True(savedToken.ExpiresAtUtc > DateTime.UtcNow);
            Assert.Null(savedToken.UsedAtUtc);
        }
        finally
        {
            await dbContext.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task VerifyEmailAsync_ShouldVerifyUserWithValidToken()
    {
        var (connection, dbContext) = await CreateDatabaseAsync();

        try
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                EmailVerified = false
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var service = new EmailVerificationService(dbContext, new FakeEmailSender());
            var token = await service.CreateVerificationTokenAsync(user);

            var result = await service.VerifyEmailAsync(token);

            Assert.True(result);
            Assert.True(user.EmailVerified);

            var savedToken = await dbContext.EmailVerificationTokens
                .SingleAsync(x => x.UserId == user.Id);

            Assert.NotNull(savedToken.UsedAtUtc);
        }
        finally
        {
            await dbContext.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task VerifyEmailAsync_ShouldRejectTokenReuse()
    {
        var (connection, dbContext) = await CreateDatabaseAsync();

        try
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                EmailVerified = false
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var service = new EmailVerificationService(dbContext, new FakeEmailSender());
            var token = await service.CreateVerificationTokenAsync(user);

            Assert.True(await service.VerifyEmailAsync(token));
            Assert.False(await service.VerifyEmailAsync(token));
        }
        finally
        {
            await dbContext.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task VerifyEmailAsync_ShouldRejectInvalidToken()
    {
        var (connection, dbContext) = await CreateDatabaseAsync();

        try
        {
            var service = new EmailVerificationService(dbContext, new FakeEmailSender());

            Assert.False(await service.VerifyEmailAsync("invalid-token"));
            Assert.False(await service.VerifyEmailAsync(""));
        }
        finally
        {
            await dbContext.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

   [Fact]
public async Task VerifyEmailAsync_ShouldRejectExpiredToken()
{
    var (connection, dbContext) = await CreateDatabaseAsync();

    try
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "expired@example.com",
            EmailVerified = false
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var service = new EmailVerificationService(
            dbContext,
            new FakeEmailSender());

        var token = await service.CreateVerificationTokenAsync(user);

        var savedToken = await dbContext.EmailVerificationTokens
            .SingleAsync(x => x.UserId == user.Id);

        savedToken.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(-1);
        await dbContext.SaveChangesAsync();

        var result = await service.VerifyEmailAsync(token);

        Assert.False(result);
        Assert.False(user.EmailVerified);
    }
    finally
    {
        await dbContext.DisposeAsync();
        await connection.DisposeAsync();
    }
}

    public class FakeEmailSender : IEmailSender
{
    public string? LastEmail { get; private set; }
    public string? LastToken { get; private set; }

    public Task SendVerificationEmailAsync(
        string email,
        string verificationToken)
    {
        LastEmail = email;
        LastToken = verificationToken;

        return Task.CompletedTask;
    }
}
}


