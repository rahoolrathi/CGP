import { useSyncExternalStore } from 'react';
import { z } from 'zod';
import { preferences } from '../../shared/storage';

/**
 * TEMPORARY until login is built: the employee picked on the "Who are you?" screen.
 * Anyone can pick any employee, so this is for testing only.
 */
const currentEmployeeSchema = z.object({
  id: z.string(),
  employeeId: z.string(),
  fullName: z.string(),
  departmentName: z.string(),
});

export type CurrentEmployee = z.infer<typeof currentEmployeeSchema>;

const listeners = new Set<() => void>();
let cached: CurrentEmployee | null | undefined;

function read(): CurrentEmployee | null {
  if (cached !== undefined) return cached;
  const raw = preferences.get('currentEmployee');
  const parsed = raw ? currentEmployeeSchema.safeParse(JSON.parse(raw)) : null;
  cached = parsed?.success ? parsed.data : null;
  return cached;
}

function notify(next: CurrentEmployee | null) {
  cached = next;
  listeners.forEach((listener) => listener());
}

export function setCurrentEmployee(employee: CurrentEmployee) {
  preferences.set('currentEmployee', JSON.stringify(employee));
  notify(employee);
}

export function clearCurrentEmployee() {
  preferences.remove('currentEmployee');
  notify(null);
}

export function useCurrentEmployee() {
  return useSyncExternalStore((listener) => {
    listeners.add(listener);
    return () => listeners.delete(listener);
  }, read);
}
