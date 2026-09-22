## Sprint 1 — Database & Multi-Tenancy

### Tenant-Aware Data Model

Market Pulse is a multi-tenant SaaS platform designed for marketing
departments, startups, and small businesses.

Each organization operates as an independent tenant with isolated
users, social accounts, campaigns, posts, comments, analytics,
AI analysis, notifications, and reports.

The organization is the primary data and security boundary.

---

### 1. High-Level Architecture

```text
                              MARKET PULSE
                                   |
                  +----------------+----------------+
                  |                                 |
                  v                                 v
          +---------------+                  +---------------+
          | MarketPulse   |                  | MarketPulse   |
          | Client        |<---------------->| API           |
          |               |                  |               |
          | Blazor        |                  | ASP.NET Core  |
          +-------+-------+                  +-------+-------+
                  |                                  |
                  |                                  +-- REST API
                  |                                  |
                  |                                  +-- SignalR
                  |                                  |
                  |                                  v
                  |                         +----------------+
                  |                         | Application    |
                  |                         | Modules        |
                  |                         |                |
                  |                         | Tenancy        |
                  |                         | Identity       |
                  |                         | Marketing      |
                  |                         | Analytics      |
                  |                         | Reporting      |
                  |                         | Billing        |
                  |                         +-------+--------+
                  |                                 |
                  |                                 v
                  +--------------------------> +------------+
                                               | Database   |
                                               +------------+
```

---

### 2. Local Tenant Development

During local development, tenant subdomains are mapped to the local
machine because production wildcard DNS is not available yet.

#### Windows hosts file

Add the following entries to:

`C:\Windows\System32\drivers\etc\hosts`

```text
127.0.0.1 acme.marketpulse.com
127.0.0.1 zimtech.marketpulse.com
```

The hosts file requires administrator privileges to edit.

#### Start the API

Run the API using the HTTP launch profile:

```powershell
dotnet run --project .\MarketPulse.Api --launch-profile http
```

The API runs on:

`http://localhost:5182`

#### Test tenant subdomains

Use the tenant hostname when making requests:

```powershell
curl.exe -i http://acme.marketpulse.com:5182/
curl.exe -i http://zimtech.marketpulse.com:5182/
```

Both hostnames should resolve to `127.0.0.1` and reach the local
ASP.NET Core API.

The tenant resolution middleware uses the subdomain to identify the
organization:

- `acme.marketpulse.com` → `acme`
- `zimtech.marketpulse.com` → `zimtech`

Unknown tenant subdomains return `404 Not Found`.

This local hosts-file approach is only for development. In production,
tenant subdomains will be handled through wildcard DNS.

---

### 3. Accepted Tenant-Isolation Rules

The following rules are accepted for the Sprint 1 data foundation:

1. Every tenant-owned entity must contain an `OrganizationId`
   identifying its owning organization.

2. Tenant-owned queries must be automatically restricted to the
   organization resolved for the current request.

3. EF Core global query filters are used to enforce tenant isolation
   for tenant-owned entities.

4. Tenant context is resolved from the tenant subdomain before
   controller actions execute.

5. A request for an unknown or inactive tenant is rejected.

6. Tenant-owned records must not be returned across organization
   boundaries.

7. Tenant-owned records must reference a valid organization through
   a foreign key.

8. Tenant isolation must be verified with automated checks or
   targeted integration testing before production deployment.

The Sprint 1 implementation has been manually verified using separate
Acme and ZimTech campaigns. Each tenant could only retrieve its own
campaign records.