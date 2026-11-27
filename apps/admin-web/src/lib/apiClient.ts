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

type Method = 'GET' | 'POST' | 'PATCH' | 'DELETE';

async function request(method: Method, path: string, body?: BodyInit, signal?: AbortSignal) {
  const headers: Record<string, string> = { Accept: 'application/json' };
  if (typeof body === 'string') headers['Content-Type'] = 'application/json';

  const response = await fetch(`/api/v1${path}`, {
    method,
    body,
    headers,
    credentials: 'same-origin',
    signal,
  });

  if (!response.ok) {
    const problem = (await response.json().catch(() => null)) as { code?: string } | null;
    throw new ApiError(response.status, problem?.code);
  }

  return response;
}

export async function apiGet<T>(path: string, schema: z.ZodType<T>, signal?: AbortSignal) {
  const response = await request('GET', path, undefined, signal);
  return schema.parse(await response.json());
}

export async function apiSend<T>(
  method: Method,
  path: string,
  schema: z.ZodType<T>,
  body?: unknown,
) {
  const response = await request(
    method,
    path,
    body === undefined ? undefined : JSON.stringify(body),
  );
  return schema.parse(await response.json());
}

export async function apiDelete(path: string) {
  await request('DELETE', path);
}

export async function apiUpload<T>(path: string, schema: z.ZodType<T>, file: File) {
  const form = new FormData();
  form.append('file', file);
  const response = await request('POST', path, form);
  return schema.parse(await response.json());
}
