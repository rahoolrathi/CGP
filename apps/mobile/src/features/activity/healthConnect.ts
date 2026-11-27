import {
  aggregateRecord,
  getGrantedPermissions,
  getSdkStatus,
  initialize,
  openHealthConnectSettings,
  requestPermission,
  SdkAvailabilityStatus,
} from 'react-native-health-connect';
import type { QatarDay } from './qatarDay';

const recordTypes = ['Steps', 'Distance', 'ActiveCaloriesBurned', 'ExerciseSession'] as const;
const permissions = recordTypes.map((recordType) => ({ accessType: 'read' as const, recordType }));

export type HealthStatus = 'unavailable' | 'needsUpdate' | 'needsPermission' | 'ready';

export interface DayTotals {
  date: string;
  steps: number;
  distanceMeters: number;
  activeMinutes: number;
  caloriesKcal: number;
  source: string | null;
}

export async function getHealthStatus(): Promise<HealthStatus> {
  const sdk = await getSdkStatus();
  if (sdk === SdkAvailabilityStatus.SDK_UNAVAILABLE) return 'unavailable';
  if (sdk === SdkAvailabilityStatus.SDK_UNAVAILABLE_PROVIDER_UPDATE_REQUIRED) return 'needsUpdate';

  await initialize();
  const granted = await getGrantedPermissions();
  const hasSteps = granted.some((p) => p.recordType === 'Steps' && p.accessType === 'read');
  return hasSteps ? 'ready' : 'needsPermission';
}

export async function askForPermission(): Promise<HealthStatus> {
  await initialize();
  await requestPermission(permissions);
  return getHealthStatus();
}

export { openHealthConnectSettings };

/** Reads the daily totals for each day from Health Connect. */
export async function readDays(days: QatarDay[]): Promise<DayTotals[]> {
  return Promise.all(days.map(readDay));
}

async function readDay(day: QatarDay): Promise<DayTotals> {
  const timeRangeFilter = {
    operator: 'between' as const,
    startTime: day.start.toISOString(),
    endTime: day.end.toISOString(),
  };

  const [steps, distance, calories, exercise] = await Promise.all([
    aggregateRecord({ recordType: 'Steps', timeRangeFilter }),
    aggregateRecord({ recordType: 'Distance', timeRangeFilter }).catch(() => null),
    aggregateRecord({ recordType: 'ActiveCaloriesBurned', timeRangeFilter }).catch(() => null),
    aggregateRecord({ recordType: 'ExerciseSession', timeRangeFilter }).catch(() => null),
  ]);

  return {
    date: day.date,
    steps: steps.COUNT_TOTAL,
    distanceMeters: Math.round(distance?.DISTANCE.inMeters ?? 0),
    activeMinutes: Math.round((exercise?.EXERCISE_DURATION_TOTAL.inSeconds ?? 0) / 60),
    caloriesKcal: Math.round(calories?.ACTIVE_CALORIES_TOTAL.inKilocalories ?? 0),
    source: steps.dataOrigins[0] ?? null,
  };
}
