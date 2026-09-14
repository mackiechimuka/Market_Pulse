using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public static class TenancySeeder
{
    public static async Task SeedAsync(MarketPulseDbContext dbContext)
    {
        if (await dbContext.Organizations.AnyAsync())
        {
            return;
        }

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = "Acme",
            Slug = "acme",
            OwnerUserId = Guid.NewGuid(),
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true
        };

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();
    }
}