using MarketPulse.Api.Modules.Marketing;
using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public class MarketPulseDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public MarketPulseDbContext(
        DbContextOptions<MarketPulseDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<Campaign> Campaigns => Set<Campaign>();

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

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.OrganizationId)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasOne<Organization>()
                .WithMany()
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.OrganizationId);

            entity.HasQueryFilter(x =>
                !_tenantContext.IsResolved ||
                x.OrganizationId == _tenantContext.OrganizationId);
        });
    }
}