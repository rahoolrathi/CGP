import { createMMKV } from 'react-native-mmkv';

type PreferenceKey = 'language' | 'theme' | 'currentEmployee';

const store = createMMKV({ id: 'cgb-preferences' });

export const preferences = {
  get: (key: PreferenceKey): string | undefined => store.getString(key),
  set: (key: PreferenceKey, value: string): void => store.set(key, value),
  remove: (key: PreferenceKey): void => {
    store.remove(key);
  },
};
