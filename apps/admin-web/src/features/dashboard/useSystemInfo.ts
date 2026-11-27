import { useQuery } from '@tanstack/react-query';
import { z } from 'zod';
import { apiGet } from '../../lib/apiClient';

const systemInfoSchema = z.object({
  version: z.string(),
  serverTimeUtc: z.string(),
  qatarDate: z.string(),
});

export type SystemInfo = z.infer<typeof systemInfoSchema>;

export function useSystemInfo() {
  return useQuery({
    queryKey: ['system', 'info'],
    queryFn: ({ signal }) => apiGet('/system/info', systemInfoSchema, signal),
  });
}
