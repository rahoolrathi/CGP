import ar from './locales/ar.json';
import en from './locales/en.json';

export const supportedLanguages = ['en', 'ar'] as const;
export type Language = (typeof supportedLanguages)[number];

export const defaultLanguage: Language = 'en';
export const defaultNamespace = 'translation';

export type TranslationResource = typeof en;

export const resources = {
  en: { [defaultNamespace]: en },
  ar: { [defaultNamespace]: ar },
} as const satisfies Record<Language, Record<string, TranslationResource>>;

const rtlLanguages: ReadonlySet<Language> = new Set(['ar']);

export function isRtl(language: Language): boolean {
  return rtlLanguages.has(language);
}

export function resolveLanguage(candidate: string | null | undefined): Language {
  const base = candidate?.toLowerCase().split(/[-_]/)[0];
  return supportedLanguages.find((lang) => lang === base) ?? defaultLanguage;
}
