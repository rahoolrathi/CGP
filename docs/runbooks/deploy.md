# Deploy runbook

## 1. Neon database

Create a project with database `cgb_steps`, then run once as the owner role in the SQL editor:

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

Create a `dev` branch from `main` for local work and for trying migrations first.

## 2. API on Railway

1. New project → Deploy from GitHub repo → set the service root directory to `backend`.
2. Railway picks up `backend/railway.json` (Dockerfile build, migrations as pre-deploy, health check).
3. Add the variables from `backend/.env.example`. Use Neon's pooled host for `ConnectionStrings__App` and the direct host for `ConnectionStrings__Migrations`.
4. Generate a public domain and check `https://<domain>/health/ready` returns `Healthy`.

## 3. Admin web on Vercel

1. Import the repo, set the root directory to `apps/admin-web` (framework: Vite).
2. Update the rewrite destination in `apps/admin-web/vercel.json` to the Railway domain.
3. Add the Vercel domain to `Cors__AllowedOrigins__0` on Railway.

## 4. Android APK

```sh
cd apps/mobile
npx eas-cli@latest login
npx eas-cli@latest build --platform android --profile preview
```

EAS creates and stores the release keystore on first build. Download a backup with `eas credentials` and keep it safe: losing it means users must uninstall to update.

## Rolling back a migration

```sh
cd backend
ConnectionStrings__Migrations="<direct connection string>" \
  dotnet ef database update <PreviousMigrationName> --project src/CgbSteps.Api
```
