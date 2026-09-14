namespace MarketPulse.Api.Modules.Tenancy;

public class TenantContext : ITenantContext
{
    public Guid? OrganizationId { get; private set; }

    public string? OrganizationSlug { get; private set; }

    public bool IsResolved => OrganizationId.HasValue;

    public void SetTenant(Guid organizationId, string organizationSlug)
    {
        OrganizationId = organizationId;
        OrganizationSlug = organizationSlug;
    }
}