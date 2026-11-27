import { supportedLanguages } from '@cgb/i18n';
import { useTranslation } from 'react-i18next';
import { changeLanguage, currentLanguage } from '../../shared/i18n';
import { themePreferences, useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';
import { OptionGroup } from '../../shared/ui/OptionGroup';
import { Screen } from '../../shared/ui/Screen';

export function SettingsScreen() {
  const { t } = useTranslation();
  const { preference, setPreference } = useTheme();

  return (
    <Screen>
      <AppText variant="headline">{t('settings.title')}</AppText>
      <Card>
        <OptionGroup
          label={t('common.language')}
          selected={currentLanguage()}
          onSelect={(language) => void changeLanguage(language)}
          options={supportedLanguages.map((value) => ({
            value,
            label: t(`settings.languages.${value}`),
          }))}
        />
      </Card>
      <Card>
        <OptionGroup
          label={t('common.theme')}
          selected={preference}
          onSelect={setPreference}
          options={themePreferences.map((value) => ({
            value,
            label: t(`settings.themes.${value}`),
          }))}
        />
      </Card>
    </Screen>
  );
}
