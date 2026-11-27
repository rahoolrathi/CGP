# 1. Hosting and deployment

- Status: Accepted
- Date: 2026-11-27

## Context

CGB Steps starts as a small internal project. It needs low-cost hosting that deploys from GitHub with little setup. The Android app is shared directly with employees, not through the Play Store.

## Decision

| Part      | Host                                   | Notes                                                                           |
| --------- | -------------------------------------- | ------------------------------------------------------------------------------- |
| API       | Railway, one Docker service            | Migrations run as the pre-deploy command. Health check on `/health/ready`.      |
| Admin web | Vercel (static Vite build)             | `/api/*` is rewritten to the Railway API so the browser sees one site.          |
| Database  | Neon Postgres (free tier)              | `main` branch for production, `dev` branch for development and migration tests. |
| Android   | APK built with EAS (`preview` profile) | Signed with our own release key, shared directly.                               |

## Consequences

- The Vercel rewrite keeps admin cookies first-party, avoiding third-party cookie blocking.
- Railway's free trial is limited; the Hobby plan costs about $5 a month. Render's free tier is a fallback but sleeps when idle.
- Moving to the Play Store later only needs an `aab` build profile.
