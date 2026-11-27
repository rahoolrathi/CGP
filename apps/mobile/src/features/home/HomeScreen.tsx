import { radius, spacing } from '@cgb/ui-tokens';
import { StyleSheet, View } from 'react-native';
import { useTranslation } from 'react-i18next';
import { useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';
import { Screen } from '../../shared/ui/Screen';
import { greetingKey } from './greeting';

const DEFAULT_DAILY_GOAL = 10_000;

export function HomeScreen() {
  const { t, i18n } = useTranslation();
  const { colors } = useTheme();
  const format = (value: number) => value.toLocaleString(i18n.language);

  return (
    <Screen>
      <AppText variant="headline">{t(greetingKey(new Date()))}</AppText>

      <Card style={styles.ringCard}>
        <View
          style={[styles.ring, { borderColor: colors.primaryMuted }]}
          accessibilityLabel={t('home.stepsToday', { count: 0 })}
        >
          <AppText variant="display">{format(0)}</AppText>
          <AppText tone="secondary">{t('home.goal', { goal: format(DEFAULT_DAILY_GOAL) })}</AppText>
        </View>
        <AppText tone="secondary" style={styles.centered}>
          {t('home.comingSoon')}
        </AppText>
      </Card>
    </Screen>
  );
}

const RING_SIZE = 200;

const styles = StyleSheet.create({
  ringCard: { alignItems: 'center', gap: spacing.lg, paddingVertical: spacing.xl },
  ring: {
    width: RING_SIZE,
    height: RING_SIZE,
    borderRadius: radius.pill,
    borderWidth: 14,
    alignItems: 'center',
    justifyContent: 'center',
  },
  centered: { textAlign: 'center' },
});
