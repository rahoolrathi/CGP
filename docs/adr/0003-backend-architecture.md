# 3. Backend architecture: Clean Architecture with Minimal APIs

- Status: Accepted
- Date: 2026-11-27

## Context

The backend must stay easy to read and test as features are added, without heavy framework ceremony.

## Decision

- Four projects with one-way dependencies: `Api → Infrastructure → Application → Domain`.
  - **Domain**: entities and rules with no framework dependencies.
  - **Application**: use cases as small handler classes (one per command or query), plus the interfaces they need.
  - **Infrastructure**: EF Core, Postgres and other external services.
  - **Api**: Minimal API endpoints grouped per feature under `/api/v1`, plus HTTP concerns (errors, auth, CORS, rate limits).
- Handlers are injected directly into endpoints. No mediator library.
- Errors use RFC 7807 Problem Details with a stable `code` the clients translate.
- Every endpoint requires authentication unless it explicitly calls `AllowAnonymous()`.
- One test project covers domain, application and API behaviour.

## Consequences

- Fewer packages and less indirection than a mediator pipeline.
- Cross-cutting behaviour (validation, logging) is added with endpoint filters when needed.
