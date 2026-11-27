import { supportedLanguages } from '@cgb/i18n';
import { minTouchTarget, radius } from '@cgb/ui-tokens';
import { router } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { Pressable, StyleSheet } from 'react-native';
import { clearCurrentEmployee, useCurrentEmployee } from '../identity/currentEmployee';
import { changeLanguage, currentLanguage } from '../../shared/i18n';
import { themePreferences, useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { Card } from '../../shared/ui/Card';
import { OptionGroup } from '../../shared/ui/OptionGroup';
import { Screen } from '../../shared/ui/Screen';

export function SettingsScreen() {
  const { t } = useTranslation();
  const { preference, setPreference, colors } = useTheme();
  const employee = useCurrentEmployee();

  const switchEmployee = () => {
    clearCurrentEmployee();
    router.replace('/pick-employee');
  };

  return (
    <Screen>
      <AppText variant="headline">{t('settings.title')}</AppText>
      {employee && (
        <Card>
          <AppText variant="title">{employee.fullName}</AppText>
          <AppText tone="secondary">
            {employee.employeeId} · {employee.departmentName}
          </AppText>
          <Pressable
            accessibilityRole="button"
            onPress={switchEmployee}
            style={({ pressed }) => [
              styles.button,
              { borderColor: colors.primary, opacity: pressed ? 0.7 : 1 },
            ]}
          >
            <AppText tone="brand">{t('identity.switch')}</AppText>
          </Pressable>
        </Card>
      )}
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

const styles = StyleSheet.create({
  button: {
    minHeight: minTouchTarget,
    borderWidth: 1,
    borderRadius: radius.pill,
    alignItems: 'center',
    justifyContent: 'center',
  },
});
