import {
  defaultLanguage,
  defaultNamespace,
  isRtl,
  resolveLanguage,
  resources,
  type Language,
} from '@cgb/i18n';
import { reloadAppAsync } from 'expo';
import { getLocales } from 'expo-localization';
import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import { I18nManager } from 'react-native';
import { preferences } from './storage';

const initialLanguage = resolveLanguage(
  preferences.get('language') ?? getLocales()[0]?.languageCode,
);

void i18n.use(initReactI18next).init({
  resources,
  lng: initialLanguage,
  fallbackLng: defaultLanguage,
  defaultNS: defaultNamespace,
  interpolation: { escapeValue: false },
});

I18nManager.allowRTL(true);
I18nManager.forceRTL(isRtl(initialLanguage));

export async function changeLanguage(language: Language) {
  preferences.set('language', language);
  await i18n.changeLanguage(language);

  const rtl = isRtl(language);
  if (I18nManager.isRTL !== rtl) {
    I18nManager.forceRTL(rtl);
    await reloadAppAsync('Layout direction changed');
  }
}

export function currentLanguage(): Language {
  return resolveLanguage(i18n.language);
}

export default i18n;
