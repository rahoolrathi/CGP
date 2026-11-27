# 4. Background jobs with Quartz.NET in-process

- Status: Accepted (package added when the first job lands in Increment 3)
- Date: 2026-11-27

## Context

Streaks, badges and rankings are recomputed on a schedule, never on request. We run a single API service on Railway.

## Decision

Run Quartz.NET inside the API process with an in-memory schedule. Each job is an idempotent recompute, so a restart loses nothing.

## Consequences

- No extra service or job tables to operate.
- With more than one API instance, jobs would run on each instance. Before scaling out, switch Quartz to its Postgres job store with clustering, or move jobs to a dedicated worker.
