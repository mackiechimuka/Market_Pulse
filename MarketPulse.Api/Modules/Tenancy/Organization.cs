namespace MarketPulse.Api.Modules.Tenancy;

public class Organization
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public Guid OwnerUserId { get; set; }

    public DateTime CreatedAtUtc { get; set; }=  DateTime.UtcNow;

    public bool IsActive { get; set; }
}