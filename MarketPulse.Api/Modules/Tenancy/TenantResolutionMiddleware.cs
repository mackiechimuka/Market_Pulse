using Microsoft.EntityFrameworkCore;

namespace MarketPulse.Api.Modules.Tenancy;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        MarketPulseDbContext dbContext,
        TenantContext tenantContext)
    {
        var host = httpContext.Request.Host.Host;

        var parts = host.Split(
            '.',
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
        {
            await _next(httpContext);
            return;
        }

        var slug = parts[0].ToLowerInvariant();

        var organization = await dbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Slug == slug &&
                x.IsActive);

        if (organization is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsync("Tenant not found.");
            return;
        }

        tenantContext.SetTenant(
            organization.Id,
            organization.Slug);

        await _next(httpContext);
    }
}