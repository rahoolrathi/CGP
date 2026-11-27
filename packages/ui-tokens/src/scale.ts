export const spacing = {
  xs: 4,
  sm: 8,
  md: 12,
  lg: 16,
  xl: 24,
  xxl: 32,
} as const;

export const radius = {
  sm: 8,
  md: 12,
  lg: 20,
  pill: 999,
} as const;

export const typography = {
  fontFamily: {
    latin: 'Inter',
    arabic: 'IBM Plex Sans Arabic',
  },
  size: {
    caption: 12,
    body: 15,
    title: 20,
    headline: 28,
    display: 40,
  },
  weight: {
    regular: '400',
    medium: '500',
    bold: '700',
  },
} as const;

export const minTouchTarget = 48;
