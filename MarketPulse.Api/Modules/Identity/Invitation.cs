using MarketPulse.Api.Modules.Tenancy;

namespace MarketPulse.Api.Modules.Identity;

public class Invitation
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "Member";

    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? AcceptedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Organization Organization { get; set; } = null!;
}
