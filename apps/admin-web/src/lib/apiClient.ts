import type { z } from 'zod';

export class ApiError extends Error {
  constructor(
    readonly status: number,
    readonly code: string | undefined,
  ) {
    super(`Request failed with status ${status}`);
    this.name = 'ApiError';
  }
}

export async function apiGet<T>(path: string, schema: z.ZodType<T>, signal?: AbortSignal) {
  const response = await fetch(`/api/v1${path}`, {
    credentials: 'same-origin',
    headers: { Accept: 'application/json' },
    signal,
  });

  if (!response.ok) {
    const problem = (await response.json().catch(() => null)) as { code?: string } | null;
    throw new ApiError(response.status, problem?.code);
  }

  return schema.parse(await response.json());
}
