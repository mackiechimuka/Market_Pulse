using MarketPulse.Api.Modules.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public static class TenancySeeder
{
    public static async Task SeedAsync(MarketPulseDbContext dbContext)
    {
        var acmeOwner = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == "owner@acme.local");

        if (acmeOwner is null)
        {
            acmeOwner = new User
            {
                Id = Guid.NewGuid(),
                Email = "owner@acme.local",
                PasswordHash = null,
                EmailVerified = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.Users.Add(acmeOwner);
        }

        var zimTechOwner = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == "owner@zimtech.local");

        if (zimTechOwner is null)
        {
            zimTechOwner = new User
            {
                Id = Guid.NewGuid(),
                Email = "owner@zimtech.local",
                PasswordHash = null,
                EmailVerified = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.Users.Add(zimTechOwner);
        }

        await dbContext.SaveChangesAsync();

        var acme = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Slug == "acme");

        if (acme is null)
        {
            acme = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Acme",
                Slug = "acme",
                OwnerUserId = acmeOwner.Id,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            dbContext.Organizations.Add(acme);
        }

        var zimTech = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Slug == "zimtech");

        if (zimTech is null)
        {
            zimTech = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "ZimTech",
                Slug = "zimtech",
                OwnerUserId = zimTechOwner.Id,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            dbContext.Organizations.Add(zimTech);
        }

        await dbContext.SaveChangesAsync();
    }
}
