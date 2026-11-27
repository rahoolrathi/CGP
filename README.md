# CGB Steps

Internal wellness app for CGB employees: step tracking, goals, badges, kudos and department challenges. Coins are for recognition and personalisation only.

## Folder structure

```
apps/
  admin-web/        React + Vite admin dashboard
    src/app/        App shell and providers
    src/features/   One folder per screen (dashboard, ...)
    src/lib/        API client, i18n, preferences
    src/theme/      Light and dark theme
  mobile/           Expo (React Native) Android app
    src/app/        Screens and navigation (expo-router)
    src/features/   One folder per feature (home, settings, ...)
    src/shared/     Reusable UI, theme, storage, i18n
packages/
  i18n/             English and Arabic strings
  ui-tokens/        Brand colours, spacing and typography
backend/          ASP.NET Core API (run it from this folder)
  Controllers/    HTTP endpoints (thin)
  Services/       Business logic
    Interfaces/   Service contracts
  Repositories/   Data access (added with the first tables)
    Interfaces/   Repository contracts
  Data/           EF Core DbContext and migration runner
  Models/         DTOs (and entities, once we add them)
  Middleware/     Error handling and security headers
  Extensions/     Startup wiring (services and pipeline)
  Helpers/        Small shared helpers and constants
```

## Prerequisites

- Node 22+ and pnpm 10 (`corepack enable`)
- .NET 10 SDK
- PostgreSQL 16 locally, or a Neon `dev` branch

## Run locally

First copy `backend/.env.example` to `backend/.env` and paste your Neon URLs into `DATABASE_URL` and `MIGRATIONS_DATABASE_URL`. The backend reads this file on startup. `.env` is git-ignored, so never commit it.

```sh
pnpm install

# Backend: http://localhost:5080, API docs at /scalar
cd backend
dotnet tool restore
dotnet run -- migrate
dotnet run

# Admin web: http://localhost:5173 (proxies /api to localhost:5080)
pnpm --filter @cgb/admin-web dev

# Mobile (dev build)
cd apps/mobile && npx expo run:android
```

## Checks

```sh
pnpm lint && pnpm typecheck && pnpm build
cd backend && dotnet build
```

## Deploy

### Database (Neon)

Create a project with database `cgb_steps`, then run once as the owner role:

```sql
CREATE ROLE cgb_migrator LOGIN PASSWORD '<strong password>';
CREATE ROLE cgb_app LOGIN PASSWORD '<different strong password>';
GRANT ALL ON DATABASE cgb_steps TO cgb_migrator;
ALTER SCHEMA public OWNER TO cgb_migrator;
REVOKE CREATE ON SCHEMA public FROM PUBLIC;
GRANT USAGE ON SCHEMA public TO cgb_app;
ALTER DEFAULT PRIVILEGES FOR ROLE cgb_migrator IN SCHEMA public
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO cgb_app;
ALTER DEFAULT PRIVILEGES FOR ROLE cgb_migrator IN SCHEMA public
  GRANT USAGE, SELECT ON SEQUENCES TO cgb_app;
```

The app role cannot change the schema; only the migrator role can.

### API

Hosting is not decided yet. On the host:

1. Set `DATABASE_URL` (Neon pooled URL) and `MIGRATIONS_DATABASE_URL` (Neon direct URL) as environment variables, the same keys as `backend/.env.example`.
2. Run `dotnet run -- migrate` in `backend` (or `dotnet CgbSteps.Api.dll migrate` from a published build) before each release.
3. Use `/health/ready` as the health check.

### Admin web (Vercel)

Set the root directory to `apps/admin-web`, point the rewrite in `apps/admin-web/vercel.json` at the API, and add the Vercel domain to `Cors__AllowedOrigins__0` on the API.

### Android APK

```sh
cd apps/mobile
npx eas-cli@latest build --platform android --profile preview
```

Back up the release keystore with `eas credentials`. Losing it means users must uninstall to update.
