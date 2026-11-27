# 3. Backend architecture: layered controllers, services and repositories

- Status: Accepted (revised 2026-11-27)
- Date: 2026-11-27

## Context

The backend must be easy for the team to navigate and test as features grow, without heavy framework ceremony. The team prefers a familiar layered layout over separate Clean Architecture projects.

## Decision

One API project, `CgbSteps.Api`, organised by layer:

```
Controllers/    HTTP only: routing, status codes, auth attributes. Call services.
Services/       Business logic. Depend on repository interfaces, never on EF Core directly.
Repositories/   Data access with EF Core.
Interfaces/     Contracts for services and repositories (registered in DI).
Models/
  Entities/     Database entities with their own invariants.
  Dtos/         Request and response shapes.
Data/           AppDbContext, entity configurations, migrator.
Migrations/     EF Core migrations.
Helpers/        Focused helpers (Qatar day maths, problem details).
Utils/          Constants and small generic utilities.
Middleware/     Security headers, global exception handler.
Extensions/     DI registration and the HTTP pipeline, keeping Program.cs short.
```

Rules:

- Dependencies flow one way: `Controllers → Services → Repositories → Data`, each through an interface.
- Controllers live under `/api/v1`. Every action requires authentication unless marked `[AllowAnonymous]`.
- Errors use RFC 7807 Problem Details with a stable `code` the clients translate.
- One test project (`tests/CgbSteps.Tests`) mirrors these folders; services are tested with fake repositories.

## Consequences

- Familiar to most .NET developers; new features follow the same five steps (entity, repository, service, DTO, controller).
- Layering is enforced by convention and review rather than project references. If the codebase grows a lot, the folders can be split into projects later without changing the code inside them.
