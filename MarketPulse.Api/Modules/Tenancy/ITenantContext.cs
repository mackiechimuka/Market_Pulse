namespace MarketPulse.Api.Modules.Tenancy;

public interface ITenantContext
{
    Guid? OrganizationId { get; }

    string? OrganizationSlug { get; }

    bool IsResolved { get; }
}