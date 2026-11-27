import { supportedLanguages, type Language } from '@cgb/i18n';
import type { ReactNode } from 'react';
import { useTranslation } from 'react-i18next';
import { changeLanguage, currentLanguage } from '../lib/i18n';
import { themePreferences, useTheme, type ThemePreference } from '../theme/ThemeProvider';

export function AppShell({ children }: { children: ReactNode }) {
  const { t, i18n } = useTranslation();
  const { preference, setPreference } = useTheme();
  const language = currentLanguage(i18n.language);

  return (
    <div className="shell">
      <header className="shell__header">
        <span className="shell__brand">{t('app.name')}</span>
        <div className="shell__controls">
          <label>
            <span className="visually-hidden">{t('common.language')}</span>
            <select
              value={language}
              onChange={(e) => void changeLanguage(e.target.value as Language)}
            >
              {supportedLanguages.map((lang) => (
                <option key={lang} value={lang}>
                  {t(`settings.languages.${lang}`)}
                </option>
              ))}
            </select>
          </label>
          <label>
            <span className="visually-hidden">{t('common.theme')}</span>
            <select
              value={preference}
              onChange={(e) => setPreference(e.target.value as ThemePreference)}
            >
              {themePreferences.map((option) => (
                <option key={option} value={option}>
                  {t(`settings.themes.${option}`)}
                </option>
              ))}
            </select>
          </label>
        </div>
      </header>
      <main className="shell__main">{children}</main>
    </div>
  );
}
