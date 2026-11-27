/** Qatar is UTC+3 all year (no daylight saving), so a "day" runs from 21:00 UTC to 21:00 UTC. */
const QATAR_OFFSET_MS = 3 * 60 * 60 * 1000;
const DAY_MS = 24 * 60 * 60 * 1000;

export interface QatarDay {
  /** yyyy-MM-dd in Qatar time */
  date: string;
  start: Date;
  end: Date;
}

/** The last `count` Qatar days, oldest first, ending with today. Today's end is "now". */
export function lastQatarDays(count: number, now = new Date()): QatarDay[] {
  const qatarNow = new Date(now.getTime() + QATAR_OFFSET_MS);
  const todayStartUtc =
    Date.UTC(qatarNow.getUTCFullYear(), qatarNow.getUTCMonth(), qatarNow.getUTCDate()) -
    QATAR_OFFSET_MS;

  return Array.from({ length: count }, (_, index) => {
    const daysAgo = count - 1 - index;
    const start = new Date(todayStartUtc - daysAgo * DAY_MS);
    const end = daysAgo === 0 ? now : new Date(start.getTime() + DAY_MS);
    const date = new Date(start.getTime() + QATAR_OFFSET_MS).toISOString().slice(0, 10);
    return { date, start, end };
  });
}
