# Threat model (lean)

Scope: Increment 0 foundation. Updated each increment.

## Assets

- Employee personal data (name, Employee ID, department, work email) and step history.
- Admin accounts and their actions.
- Database credentials and future signing keys.

## Trust boundaries

1. Phone app ↔ API (public internet, TLS).
2. Admin browser ↔ Vercel ↔ API (rewrite keeps one origin).
3. API ↔ Neon Postgres (TLS, `SSL Mode=VerifyFull`).

## STRIDE summary

| Threat                 | Example                       | Mitigation now                                                                            | Planned                                                  |
| ---------------------- | ----------------------------- | ----------------------------------------------------------------------------------------- | -------------------------------------------------------- |
| Spoofing               | Non-employee signs up         | All routes deny by default                                                                | Roster check plus second proof, throttling (Inc 1)       |
| Tampering              | Fake step counts              | Server-side validation only                                                               | Health Connect origin checks, anomaly flags (Inc 2)      |
| Repudiation            | Admin denies a change         | `audit_log` table exists                                                                  | Write audit entries for every admin action (Inc 1+)      |
| Information disclosure | Leaked secrets or PII in logs | Secrets only in host env vars, gitleaks in CI, no request bodies logged, security headers | "Hide my steps" enforced server-side (Inc 4)             |
| Denial of service      | Request flooding              | Per-IP rate limit, health checks                                                          | Stricter limits on auth endpoints (Inc 1)                |
| Elevation of privilege | Employee calls admin API      | Fallback authorization policy                                                             | Role and resource checks with tests per endpoint (Inc 1) |

## Database roles

- `cgb_migrator` owns the schema and runs migrations only.
- `cgb_app` (runtime) can read and write rows but cannot create, alter or drop tables.

## Parked for later

Key vault, certificate pinning, CodeQL and container scans, OpenTelemetry and Sentry, pen-test, ASVS/MASVS mapping.
