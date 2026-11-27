import { colors, type ColorScheme, type ThemeColors } from '@cgb/ui-tokens';
import { createContext, useContext, useMemo, useState, type ReactNode } from 'react';
import { useColorScheme } from 'react-native';
import { preferences } from '../storage';

export const themePreferences = ['system', 'light', 'dark'] as const;
export type ThemePreference = (typeof themePreferences)[number];

interface ThemeContextValue {
  preference: ThemePreference;
  scheme: ColorScheme;
  colors: ThemeColors;
  setPreference: (preference: ThemePreference) => void;
}

const ThemeContext = createContext<ThemeContextValue | null>(null);

function readPreference(): ThemePreference {
  const stored = preferences.get('theme');
  return themePreferences.find((p) => p === stored) ?? 'system';
}

export function ThemeProvider({ children }: { children: ReactNode }) {
  const [preference, setPreferenceState] = useState<ThemePreference>(readPreference);
  const system = useColorScheme() === 'dark' ? 'dark' : 'light';
  const scheme = preference === 'system' ? system : preference;

  const value = useMemo<ThemeContextValue>(
    () => ({
      preference,
      scheme,
      colors: colors[scheme],
      setPreference: (next) => {
        preferences.set('theme', next);
        setPreferenceState(next);
      },
    }),
    [preference, scheme],
  );

  return <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>;
}

export function useTheme() {
  const context = useContext(ThemeContext);
  if (!context) throw new Error('useTheme must be used inside ThemeProvider');
  return context;
}
