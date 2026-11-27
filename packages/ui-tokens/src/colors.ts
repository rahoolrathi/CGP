export const palette = {
  maroon50: '#FBEFF3',
  maroon100: '#F3D3DE',
  maroon300: '#C9617F',
  maroon500: '#8A1538',
  maroon700: '#5E0E26',
  gold300: '#F1D58A',
  gold500: '#C9A227',
  gold700: '#8C6F12',
  neutral0: '#FFFFFF',
  neutral50: '#F7F6F5',
  neutral100: '#EDEBEA',
  neutral300: '#C7C3C1',
  neutral500: '#8A8482',
  neutral700: '#45403E',
  neutral800: '#2A2625',
  neutral900: '#181514',
  success: '#2E7D4F',
  danger: '#C62828',
} as const;

export type ColorScheme = 'light' | 'dark';

export interface ThemeColors {
  background: string;
  surface: string;
  surfaceMuted: string;
  border: string;
  textPrimary: string;
  textSecondary: string;
  primary: string;
  onPrimary: string;
  primaryMuted: string;
  coin: string;
  success: string;
  danger: string;
}

export const colors: Record<ColorScheme, ThemeColors> = {
  light: {
    background: palette.neutral50,
    surface: palette.neutral0,
    surfaceMuted: palette.neutral100,
    border: palette.neutral300,
    textPrimary: palette.neutral900,
    textSecondary: palette.neutral700,
    primary: palette.maroon500,
    onPrimary: palette.neutral0,
    primaryMuted: palette.maroon100,
    coin: palette.gold500,
    success: palette.success,
    danger: palette.danger,
  },
  dark: {
    background: palette.neutral900,
    surface: palette.neutral800,
    surfaceMuted: palette.neutral700,
    border: palette.neutral700,
    textPrimary: palette.neutral50,
    textSecondary: palette.neutral300,
    primary: palette.maroon300,
    onPrimary: palette.neutral900,
    primaryMuted: palette.maroon700,
    coin: palette.gold300,
    success: '#5FB383',
    danger: '#EF6B6B',
  },
};
