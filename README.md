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
