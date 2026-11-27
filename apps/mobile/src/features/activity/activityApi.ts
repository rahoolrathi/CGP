import { z } from 'zod';
import { apiRequest } from '../../shared/api';
import type { DayTotals } from './healthConnect';

const activityDaySchema = z.object({
  date: z.string(),
  steps: z.number(),
  distanceMeters: z.number(),
  activeMinutes: z.number(),
  caloriesKcal: z.number(),
  isFlagged: z.boolean(),
});

export type ActivityDay = z.infer<typeof activityDaySchema>;

/** Saves daily totals. The server replaces days it already has, so retrying is safe. */
export function syncActivity(employeeId: string, days: DayTotals[]) {
  return apiRequest('PUT', `/employees/${employeeId}/activity`, z.array(activityDaySchema), {
    days,
  });
}

export function getActivity(employeeId: string) {
  return apiRequest('GET', `/employees/${employeeId}/activity`, z.array(activityDaySchema));
}
