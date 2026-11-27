import { z } from 'zod';
import { apiRequest } from '../../shared/api';

const employeeSchema = z.object({
  id: z.string(),
  employeeId: z.string(),
  fullName: z.string(),
  departmentName: z.string(),
});

const employeePageSchema = z.object({ items: z.array(employeeSchema) });

export type EmployeeSummary = z.infer<typeof employeeSchema>;

export async function searchEmployees(search: string) {
  const params = new URLSearchParams({ pageSize: '30' });
  if (search.trim()) params.set('search', search.trim());
  const page = await apiRequest('GET', `/employees?${params.toString()}`, employeePageSchema);
  return page.items;
}
