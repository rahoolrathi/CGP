import {
  defaultLanguage,
  defaultNamespace,
  isRtl,
  resolveLanguage,
  resources,
  type Language,
} from '@cgb/i18n';
import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import { preferences } from './preferences';

function applyDocumentDirection(language: Language) {
  document.documentElement.lang = language;
  document.documentElement.dir = isRtl(language) ? 'rtl' : 'ltr';
}

const initialLanguage = resolveLanguage(preferences.get('language') ?? navigator.language);

void i18n.use(initReactI18next).init({
  resources,
  lng: initialLanguage,
  fallbackLng: defaultLanguage,
  defaultNS: defaultNamespace,
  interpolation: { escapeValue: false },
});

applyDocumentDirection(initialLanguage);

export function currentLanguage(value: string): Language {
  return resolveLanguage(value);
}

export async function changeLanguage(language: Language) {
  await i18n.changeLanguage(language);
  preferences.set('language', language);
  applyDocumentDirection(language);
}

export default i18n;
