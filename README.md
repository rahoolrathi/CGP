# CGB Steps

Internal wellness app for CGB employees: step tracking, goals, badges, kudos and department challenges. Coins are for recognition and personalisation only.

## Structure

```
apps/
  mobile/        Expo (React Native) Android app
  admin-web/     React + Vite admin dashboard
packages/
  ui-tokens/     Brand colours, spacing and typography
  i18n/          English and Arabic strings
backend/         ASP.NET Core API (Domain, Application, Infrastructure, Api)
docs/            ADRs, threat model, runbooks
```

## Prerequisites

- Node 22+ and pnpm 10 (`corepack enable`)
- .NET 10 SDK
- PostgreSQL 16 locally, or a Neon `dev` branch

## Getting started

```sh
pnpm install

# Backend (create the cgb_migrator and cgb_app roles first, see docs/runbooks/deploy.md)
cd backend
dotnet tool restore
dotnet run --project src/CgbSteps.Api -- migrate
dotnet run --project src/CgbSteps.Api      # http://localhost:5080, API docs at /scalar

# Admin web (proxies /api to localhost:5080)
pnpm --filter @cgb/admin-web dev           # http://localhost:5173

# Mobile (needs a dev build because of Health Connect)
cd apps/mobile && npx expo run:android
```

## Quality checks

```sh
pnpm lint && pnpm typecheck && pnpm test && pnpm build
cd backend && dotnet build CgbSteps.slnx && dotnet test --solution CgbSteps.slnx
```

## Conventions

- Conventional Commits (`feat:`, `fix:`, `chore:`, `docs:` …).
- Every PR fills in the "Security considerations" section.
- Every user-facing string goes through `packages/i18n` in both English and Arabic.
- New dependencies are justified in the PR description.
- Significant decisions get an ADR in `docs/adr/`.

## Deployment

See [docs/runbooks/deploy.md](docs/runbooks/deploy.md): API on Railway, admin web on Vercel, database on Neon, Android as a shared APK.
