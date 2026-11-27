import { keepPreviousData, useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { z } from 'zod';
import { apiDelete, apiGet, apiSend, apiUpload } from '../../lib/apiClient';
import { departmentsKey } from '../departments/departmentsApi';

const employeeSchema = z.object({
  id: z.string(),
  employeeId: z.string(),
  fullName: z.string(),
  workEmail: z.string(),
  departmentId: z.string(),
  departmentName: z.string(),
  createdAt: z.string(),
});

const employeePageSchema = z.object({
  items: z.array(employeeSchema),
  page: z.number(),
  pageSize: z.number(),
  totalCount: z.number(),
});

const importResultSchema = z.object({
  added: z.number(),
  skipped: z.number(),
  departmentsCreated: z.number(),
  errors: z.array(z.object({ row: z.number(), code: z.string() })),
});

export type Employee = z.infer<typeof employeeSchema>;
export type ImportResult = z.infer<typeof importResultSchema>;

export interface EmployeeFilters {
  search: string;
  departmentId: string;
  page: number;
}

export interface NewEmployee {
  employeeId: string;
  fullName: string;
  workEmail: string;
  departmentId: string;
}

export const pageSize = 20;
const employeesKey = ['employees'] as const;

export function useEmployees({ search, departmentId, page }: EmployeeFilters) {
  const params = new URLSearchParams({ page: String(page), pageSize: String(pageSize) });
  if (search.trim()) params.set('search', search.trim());
  if (departmentId) params.set('departmentId', departmentId);

  return useQuery({
    queryKey: [...employeesKey, { search, departmentId, page }],
    queryFn: ({ signal }) => apiGet(`/employees?${params}`, employeePageSchema, signal),
    placeholderData: keepPreviousData,
  });
}

/** After any change, refresh employees and the department counts. */
function useRefresh() {
  const queryClient = useQueryClient();
  return () =>
    Promise.all([
      queryClient.invalidateQueries({ queryKey: employeesKey }),
      queryClient.invalidateQueries({ queryKey: departmentsKey }),
    ]);
}

export function useAddEmployee() {
  const refresh = useRefresh();
  return useMutation({
    mutationFn: (employee: NewEmployee) => apiSend('POST', '/employees', employeeSchema, employee),
    onSuccess: refresh,
  });
}

export function useChangeDepartment() {
  const refresh = useRefresh();
  return useMutation({
    mutationFn: ({ id, departmentId }: { id: string; departmentId: string }) =>
      apiSend('PATCH', `/employees/${id}/department`, employeeSchema, { departmentId }),
    onSuccess: refresh,
  });
}

export function useDeleteEmployee() {
  const refresh = useRefresh();
  return useMutation({
    mutationFn: (id: string) => apiDelete(`/employees/${id}`),
    onSuccess: refresh,
  });
}

export function useImportRoster() {
  const refresh = useRefresh();
  return useMutation({
    mutationFn: (file: File) => apiUpload('/employees/import', importResultSchema, file),
    onSuccess: refresh,
  });
}
