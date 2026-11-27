import { minTouchTarget, radius, spacing } from '@cgb/ui-tokens';
import { useTranslation } from 'react-i18next';
import { Pressable, StyleSheet, View } from 'react-native';
import { useActivity } from '../activity/useActivity';
import { openHealthConnectSettings } from '../activity/healthConnect';
import type { CurrentEmployee } from '../identity/currentEmployee';
import { useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';
import { Screen } from '../../shared/ui/Screen';
import { GoalRing } from './GoalRing';
import { greetingKey } from './greeting';
import { StatTile } from './StatTile';
import { WeekBars } from './WeekBars';

const DEFAULT_DAILY_GOAL = 10_000;

export function HomeScreen({ employee }: { employee: CurrentEmployee }) {
  const { t, i18n } = useTranslation();
  const { colors } = useTheme();
  const { health, days, today, sync, connect } = useActivity(employee.id);
  const format = (value: number, digits = 0) =>
    value.toLocaleString(i18n.language, { maximumFractionDigits: digits });

  const steps = today?.steps ?? 0;
  const firstName = employee.fullName.split(' ')[0] ?? employee.fullName;

  return (
    <Screen>
      <View>
        <AppText variant="headline">{`${t(greetingKey(new Date()))}, ${firstName}`}</AppText>
        <AppText tone="secondary">{t(`home.sync.${sync}`)}</AppText>
      </View>

      {health !== 'ready' && health !== 'checking' && (
        <Card>
          <AppText variant="title">{t(`home.health.${health}.title`)}</AppText>
          <AppText tone="secondary">{t(`home.health.${health}.body`)}</AppText>
          <Pressable
            accessibilityRole="button"
            onPress={() =>
              health === 'needsPermission' ? void connect() : openHealthConnectSettings()
            }
            style={({ pressed }) => [
              styles.button,
              { backgroundColor: colors.primary, opacity: pressed ? 0.8 : 1 },
            ]}
          >
            <AppText style={{ color: colors.onPrimary }}>
              {t(`home.health.${health}.action`)}
            </AppText>
          </Pressable>
        </Card>
      )}

      <Card style={styles.ringCard}>
        <GoalRing
          steps={steps}
          goal={DEFAULT_DAILY_GOAL}
          label={format(steps)}
          caption={t('home.goal', { goal: format(DEFAULT_DAILY_GOAL) })}
        />
        {steps >= DEFAULT_DAILY_GOAL && (
          <AppText tone="brand" variant="title">
            {t('home.goalReached')}
          </AppText>
        )}
      </Card>

      <View style={styles.stats}>
        <StatTile
          value={format((today?.distanceMeters ?? 0) / 1000, 1)}
          label={t('home.stats.km')}
        />
        <StatTile value={format(today?.activeMinutes ?? 0)} label={t('home.stats.activeMinutes')} />
        <StatTile value={format(today?.caloriesKcal ?? 0)} label={t('home.stats.calories')} />
      </View>

      {days.length > 0 && (
        <WeekBars days={days} goal={DEFAULT_DAILY_GOAL} title={t('home.lastSevenDays')} />
      )}
    </Screen>
  );
}

const styles = StyleSheet.create({
  ringCard: { alignItems: 'center', gap: spacing.md, paddingVertical: spacing.xl },
  stats: { flexDirection: 'row', gap: spacing.sm },
  button: {
    minHeight: minTouchTarget,
    borderRadius: radius.pill,
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: spacing.sm,
  },
});
