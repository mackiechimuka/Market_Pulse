using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public class MarketPulseDbContext : DbContext
{
    public MarketPulseDbContext(DbContextOptions<MarketPulseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.Property(x => x.OwnerUserId)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();
        });
    }
}