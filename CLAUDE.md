# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Run Commands

```bash
# Build the solution
dotnet build ExpenseTracker.slnx

# Run all tests
dotnet test ExpenseTracker.slnx

# Run tests for a specific module
dotnet test src/ExpenseModule/ExpenseTracker.Expenses.Tests/ExpenseTracker.Expenses.Tests.csproj

# Run a single test by name
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run the application with Aspire (recommended for local development)
dotnet run --project src/Aspire/ExpenseTracker.AppHost/ExpenseTracker.AppHost.csproj

# Run just the WebAPI
dotnet run --project src/ExpenseTracker.WebAPI/ExpenseTracker.WebAPI.csproj

# Add EF Core migration (run from repository root)
dotnet ef migrations add MigrationName --project src/ExpenseModule/ExpenseTracker.Expenses --context ExpenseDbContext --startup-project src/ExpenseTracker.WebAPI
```

## Architecture Overview

This is a modular monolith expense tracking application built with .NET 10, using .NET Aspire for orchestration.

### Solution Structure

- **ExpenseTracker.AppHost** - Aspire orchestration host that manages PostgreSQL, migrations, WebAPI, and UI
- **ExpenseTracker.WebAPI** - Main API host, aggregates all modules via FastEndpoints
- **ExpenseTracker.MigrationService** - Worker service that runs EF Core migrations on startup
- **ExpenseTrackerUI** - Blazor frontend

### Domain Modules

Each module follows a consistent pattern with its own DbContext:

- **ExpenseModule** (`ExpenseTracker.Expenses`) - Core expense management
- **ReceiptModule** (`ExpenseTracker.Receipts`) - Receipt attachments for expenses
- **TenantModule** (`ExpenseTracker.Tenants`) - Multi-tenant support
- **UsersModule** (`ExpenseTracker.Users`) - User management with ASP.NET Identity

Each module has:

- Main project with domain logic, endpoints, and data access
- Contracts project for cross-module communication (MediatR commands/queries)
- Tests project with unit and integration tests

### Key Patterns

**Endpoint Organization**: Each endpoint has its own folder under `Endpoints/` containing:

- `*Endpoint.cs` - FastEndpoints endpoint class
- `*Request.cs` / `*Response.cs` - DTOs
- `I*Service.cs` and `*Service.cs` - Business logic
- `I*Repository.cs` and `*Repository.cs` - Data access

**Module Registration**: Modules register their services in `*ServiceExtensions.cs` and `*RepositoryExtensions.cs`, called from WebAPI's `Program.cs`.

**Cross-Module Communication**: Modules communicate via MediatR commands defined in `.Contracts` projects.

**Testing**: Integration tests use Testcontainers with PostgreSQL and a custom `TestAuthHandler` for authentication bypass.

## Code Style

- .NET 10 with file-scoped namespaces (enforced via .editorconfig)
- Async methods must have `Async` suffix (enforced as error)
- Private fields use underscore prefix (`_fieldName`)
- Warnings are treated as errors (`TreatWarningsAsErrors`)

**Pagination**: Cursor-based pagination is used
