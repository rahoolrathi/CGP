import type { z } from 'zod';

const baseUrl = (process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5080').replace(/\/$/, '');

export class ApiError extends Error {
  constructor(
    readonly status: number,
    readonly code: string | undefined,
  ) {
    super(`Request failed with status ${status}`);
    this.name = 'ApiError';
  }
}

export async function apiRequest<T>(
  method: 'GET' | 'PUT',
  path: string,
  schema: z.ZodType<T>,
  body?: unknown,
): Promise<T> {
  const response = await fetch(`${baseUrl}/api/v1${path}`, {
    method,
    headers: {
      Accept: 'application/json',
      ...(body === undefined ? {} : { 'Content-Type': 'application/json' }),
    },
    body: body === undefined ? undefined : JSON.stringify(body),
  });

  if (!response.ok) {
    const problem = (await response.json().catch(() => null)) as { code?: string } | null;
    throw new ApiError(response.status, problem?.code);
  }

  return schema.parse(await response.json());
}
