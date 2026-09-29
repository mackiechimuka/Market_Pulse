using MarketPulse.Api.Modules.Tenancy;

namespace MarketPulse.Api.Modules.Identity;

public class OrganizationMembership
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid OrganizationId { get; set; }

    public string Role { get; set; } = "Member";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public Organization Organization { get; set; } = null!;
}
