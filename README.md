# MarketPulse

MarketPulse is a modular, multi-tenant SaaS marketing platform built with ASP.NET Core and Blazor.

It is designed to let multiple organisations securely use one platform while keeping each organisation’s data isolated. MarketPulse will bring campaign management, analytics, reporting, billing, user access,AI-assisted marketing tools into one central workspace, and multilingual user interface and localization

## Architecture

MarketPulse uses a modular-monolith architecture: the platform is one deployable application, while its business areas remain cleanly separated into modules. This keeps development straightforward now and supports sustainable growth as the platform evolves.

## Solution Structure

- `MarketPulse.Api` — ASP.NET Core Web API and backend modules
- `MarketPulse.Client` — Blazor WebAssembly frontend
- `MarketPulse.Shared` — shared contracts, abstractions, and foundational types

## Current Status

Sprint 0 is complete.

The project foundation includes the solution structure, modular folders, project references, local-secret support, source-control setup, and verified API/client startup.

## Planned Modules

- Multi-tenancy
- Identity and access management
- Campaign and marketing management
- Analytics
- Reporting
- Billing
- AI-assisted marketing tools
- Multilingual user interface and localisation

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Blazor WebAssembly
- C#
- Git and GitHub

## License

Proprietary — All Rights Reserved.

This repository and its contents may not be copied, modified, distributed, or used without explicit written permission from the copyright owner.
