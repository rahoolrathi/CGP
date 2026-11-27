import { spacing } from '@cgb/ui-tokens';
import { StyleSheet } from 'react-native';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';

export function StatTile({ value, label }: { value: string; label: string }) {
  return (
    <Card style={styles.tile}>
      <AppText variant="title">{value}</AppText>
      <AppText variant="caption" tone="secondary">
        {label}
      </AppText>
    </Card>
  );
}

const styles = StyleSheet.create({
  tile: { flex: 1, alignItems: 'center', gap: spacing.xs, paddingHorizontal: spacing.sm },
});
