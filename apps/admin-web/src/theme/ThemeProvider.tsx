import { colors, type ColorScheme, type ThemeColors } from '@cgb/ui-tokens';
import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
  useSyncExternalStore,
  type ReactNode,
} from 'react';
import { preferences } from '../lib/preferences';

export const themePreferences = ['system', 'light', 'dark'] as const;
export type ThemePreference = (typeof themePreferences)[number];

interface ThemeContextValue {
  preference: ThemePreference;
  scheme: ColorScheme;
  setPreference: (preference: ThemePreference) => void;
}

const ThemeContext = createContext<ThemeContextValue | null>(null);
const darkQuery = '(prefers-color-scheme: dark)';

function subscribeToSystemScheme(onChange: () => void) {
  const media = window.matchMedia(darkQuery);
  media.addEventListener('change', onChange);
  return () => media.removeEventListener('change', onChange);
}

function systemScheme(): ColorScheme {
  return window.matchMedia(darkQuery).matches ? 'dark' : 'light';
}

function toCssVariable(token: string) {
  return `--color-${token.replace(/[A-Z]/g, (c) => `-${c.toLowerCase()}`)}`;
}

function applyScheme(scheme: ColorScheme) {
  const root = document.documentElement;
  for (const [token, value] of Object.entries(colors[scheme]) as [keyof ThemeColors, string][]) {
    root.style.setProperty(toCssVariable(token), value);
  }
  root.dataset.theme = scheme;
  root.style.colorScheme = scheme;
}

function readPreference(): ThemePreference {
  const stored = preferences.get('theme');
  return themePreferences.find((p) => p === stored) ?? 'system';
}

export function ThemeProvider({ children }: { children: ReactNode }) {
  const [preference, setPreferenceState] = useState<ThemePreference>(readPreference);
  const system = useSyncExternalStore(subscribeToSystemScheme, systemScheme);
  const scheme = preference === 'system' ? system : preference;

  useEffect(() => applyScheme(scheme), [scheme]);

  const value = useMemo<ThemeContextValue>(
    () => ({
      preference,
      scheme,
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
