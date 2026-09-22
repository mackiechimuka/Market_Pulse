using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public static class TenancySeeder
{
    public static async Task SeedAsync(MarketPulseDbContext dbContext)
    {
        var acme = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Slug == "acme");

        if (acme is null)
        {
            acme = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Acme",
                Slug = "acme",
                OwnerUserId = Guid.NewGuid(),
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
                OwnerUserId = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            dbContext.Organizations.Add(zimTech);
        }

        await dbContext.SaveChangesAsync();
    }
}