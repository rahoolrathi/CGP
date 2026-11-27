import { typography } from '@cgb/ui-tokens';
import { StyleSheet, Text, type TextProps } from 'react-native';
import { useTheme } from '../theme/ThemeProvider';

type Variant = 'display' | 'headline' | 'title' | 'body' | 'caption';
type Tone = 'primary' | 'secondary' | 'brand';

interface AppTextProps extends TextProps {
  variant?: Variant;
  tone?: Tone;
}

export function AppText({ variant = 'body', tone = 'primary', style, ...rest }: AppTextProps) {
  const { colors } = useTheme();
  const color = {
    primary: colors.textPrimary,
    secondary: colors.textSecondary,
    brand: colors.primary,
  }[tone];

  return <Text style={[styles.base, styles[variant], { color }, style]} {...rest} />;
}

const styles = StyleSheet.create({
  base: { textAlign: 'auto', writingDirection: 'auto' },
  display: { fontSize: typography.size.display, fontWeight: typography.weight.bold },
  headline: { fontSize: typography.size.headline, fontWeight: typography.weight.bold },
  title: { fontSize: typography.size.title, fontWeight: typography.weight.medium },
  body: { fontSize: typography.size.body },
  caption: { fontSize: typography.size.caption },
});
