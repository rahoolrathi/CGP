type PreferenceKey = 'language' | 'theme';

const prefix = 'cgb.pref.';

export const preferences = {
  get(key: PreferenceKey): string | null {
    try {
      return window.localStorage.getItem(prefix + key);
    } catch {
      return null;
    }
  },
  set(key: PreferenceKey, value: string): void {
    try {
      window.localStorage.setItem(prefix + key, value);
    } catch {
      /* storage unavailable: preference lasts for this session only */
    }
  },
};
