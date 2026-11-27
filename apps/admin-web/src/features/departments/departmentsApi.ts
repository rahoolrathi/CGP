import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { z } from 'zod';
import { apiDelete, apiGet, apiSend } from '../../lib/apiClient';

const departmentSchema = z.object({
  id: z.string(),
  name: z.string(),
  employeeCount: z.number(),
  createdAt: z.string(),
});

export type Department = z.infer<typeof departmentSchema>;

export const departmentsKey = ['departments'] as const;

export function useDepartments() {
  return useQuery({
    queryKey: departmentsKey,
    queryFn: ({ signal }) => apiGet('/departments', z.array(departmentSchema), signal),
  });
}

export function useAddDepartment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (name: string) => apiSend('POST', '/departments', departmentSchema, { name }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: departmentsKey }),
  });
}

export function useDeleteDepartment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => apiDelete(`/departments/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: departmentsKey }),
  });
}
