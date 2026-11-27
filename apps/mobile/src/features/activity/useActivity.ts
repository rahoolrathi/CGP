import { useFocusEffect } from 'expo-router';
import { useCallback, useState } from 'react';
import { syncActivity, type ActivityDay } from './activityApi';
import {
  askForPermission,
  getHealthStatus,
  readDays,
  type DayTotals,
  type HealthStatus,
} from './healthConnect';
import { lastQatarDays } from './qatarDay';

const DAYS_TO_SYNC = 7;

type SyncState = 'idle' | 'syncing' | 'synced' | 'offline';

/**
 * Reads the last 7 days from Health Connect, shows them right away,
 * then sends them to the server. Runs each time the screen comes into focus.
 */
export function useActivity(employeeId: string) {
  const [health, setHealth] = useState<HealthStatus | 'checking'>('checking');
  const [days, setDays] = useState<DayTotals[]>([]);
  const [sync, setSync] = useState<SyncState>('idle');
  const [flagged, setFlagged] = useState<ActivityDay[]>([]);

  const refresh = useCallback(async () => {
    const status = await getHealthStatus().catch((): HealthStatus => 'unavailable');
    setHealth(status);
    if (status !== 'ready') return;

    const totals = await readDays(lastQatarDays(DAYS_TO_SYNC));
    setDays(totals);

    setSync('syncing');
    try {
      const saved = await syncActivity(employeeId, totals);
      setFlagged(saved.filter((day) => day.isFlagged));
      setSync('synced');
    } catch {
      setSync('offline');
    }
  }, [employeeId]);

  useFocusEffect(
    useCallback(() => {
      void refresh();
    }, [refresh]),
  );

  const connect = useCallback(async () => {
    const status = await askForPermission();
    setHealth(status);
    if (status === 'ready') await refresh();
  }, [refresh]);

  const today = days.at(-1);

  return { health, days, today, sync, flagged, refresh, connect };
}
