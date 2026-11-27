import { radius, spacing } from '@cgb/ui-tokens';
import { useTranslation } from 'react-i18next';
import { StyleSheet, View } from 'react-native';
import type { DayTotals } from '../activity/healthConnect';
import { useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';

const BAR_HEIGHT = 96;

/** Simple bar chart of the last days' steps; gold bars reached the goal. */
export function WeekBars({
  days,
  goal,
  title,
}: {
  days: DayTotals[];
  goal: number;
  title: string;
}) {
  const { i18n } = useTranslation();
  const { colors } = useTheme();
  const max = Math.max(goal, ...days.map((d) => d.steps));

  return (
    <Card>
      <AppText variant="title">{title}</AppText>
      <View style={styles.row}>
        {days.map((day) => {
          const weekday = new Date(`${day.date}T12:00:00Z`).toLocaleDateString(i18n.language, {
            weekday: 'narrow',
            timeZone: 'Asia/Qatar',
          });
          return (
            <View
              key={day.date}
              style={styles.column}
              accessible
              accessibilityLabel={`${day.date}: ${day.steps.toLocaleString(i18n.language)}`}
            >
              <View style={[styles.track, { backgroundColor: colors.surfaceMuted }]}>
                <View
                  style={[
                    styles.bar,
                    {
                      height: Math.max(4, (day.steps / max) * BAR_HEIGHT),
                      backgroundColor: day.steps >= goal ? colors.coin : colors.primary,
                    },
                  ]}
                />
              </View>
              <AppText variant="caption" tone="secondary">
                {weekday}
              </AppText>
            </View>
          );
        })}
      </View>
    </Card>
  );
}

const styles = StyleSheet.create({
  row: { flexDirection: 'row', justifyContent: 'space-between', marginTop: spacing.sm },
  column: { alignItems: 'center', gap: spacing.xs, flex: 1 },
  track: {
    width: 14,
    height: BAR_HEIGHT,
    borderRadius: radius.sm,
    justifyContent: 'flex-end',
    overflow: 'hidden',
  },
  bar: { width: '100%', borderRadius: radius.sm },
});
