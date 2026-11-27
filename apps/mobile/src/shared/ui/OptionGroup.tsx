import { minTouchTarget, radius, spacing } from '@cgb/ui-tokens';
import { Pressable, StyleSheet, View } from 'react-native';
import { useTheme } from '../theme/ThemeProvider';
import { AppText } from './AppText';

interface Option<T extends string> {
  value: T;
  label: string;
}

interface OptionGroupProps<T extends string> {
  label: string;
  options: readonly Option<T>[];
  selected: T;
  onSelect: (value: T) => void;
}

export function OptionGroup<T extends string>({
  label,
  options,
  selected,
  onSelect,
}: OptionGroupProps<T>) {
  const { colors } = useTheme();

  return (
    <View style={styles.group} accessibilityRole="radiogroup" accessibilityLabel={label}>
      <AppText variant="caption" tone="secondary">
        {label}
      </AppText>
      <View style={styles.row}>
        {options.map((option) => {
          const isSelected = option.value === selected;
          return (
            <Pressable
              key={option.value}
              accessibilityRole="radio"
              accessibilityState={{ selected: isSelected }}
              onPress={() => onSelect(option.value)}
              style={({ pressed }) => [
                styles.option,
                {
                  backgroundColor: isSelected ? colors.primary : colors.surfaceMuted,
                  opacity: pressed ? 0.8 : 1,
                },
              ]}
            >
              <AppText style={{ color: isSelected ? colors.onPrimary : colors.textPrimary }}>
                {option.label}
              </AppText>
            </Pressable>
          );
        })}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  group: { gap: spacing.sm },
  row: { flexDirection: 'row', flexWrap: 'wrap', gap: spacing.sm },
  option: {
    minHeight: minTouchTarget,
    minWidth: minTouchTarget,
    paddingHorizontal: spacing.lg,
    borderRadius: radius.pill,
    alignItems: 'center',
    justifyContent: 'center',
  },
});
