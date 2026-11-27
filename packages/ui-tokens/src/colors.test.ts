import { describe, expect, it } from 'vitest';
import { colors } from './colors';

function luminance(hex: string): number {
  const channels = [1, 3, 5].map((i) => parseInt(hex.slice(i, i + 2), 16) / 255);
  const [r = 0, g = 0, b = 0] = channels.map((c) =>
    c <= 0.03928 ? c / 12.92 : ((c + 0.055) / 1.055) ** 2.4,
  );
  return 0.2126 * r + 0.7152 * g + 0.0722 * b;
}

function contrast(a: string, b: string): number {
  const [hi, lo] = [luminance(a), luminance(b)].sort((x, y) => y - x) as [number, number];
  return (hi + 0.05) / (lo + 0.05);
}

describe.each(['light', 'dark'] as const)('%s theme', (scheme) => {
  const theme = colors[scheme];

  it('keeps body text at WCAG AA contrast', () => {
    expect(contrast(theme.textPrimary, theme.background)).toBeGreaterThanOrEqual(4.5);
    expect(contrast(theme.textSecondary, theme.surface)).toBeGreaterThanOrEqual(4.5);
  });

  it('keeps primary buttons readable', () => {
    expect(contrast(theme.onPrimary, theme.primary)).toBeGreaterThanOrEqual(4.5);
  });
});
