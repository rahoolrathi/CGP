import { Redirect, Tabs } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { useCurrentEmployee } from '../../features/identity/currentEmployee';
import { useTheme } from '../../shared/theme/ThemeProvider';

export default function TabsLayout() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const employee = useCurrentEmployee();

  // TEMPORARY until login: ask who is using the app first.
  if (!employee) return <Redirect href="/pick-employee" />;

  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarActiveTintColor: colors.primary,
        tabBarInactiveTintColor: colors.textSecondary,
        tabBarStyle: { backgroundColor: colors.surface, borderTopColor: colors.border },
      }}
    >
      <Tabs.Screen name="index" options={{ title: t('tabs.home') }} />
      <Tabs.Screen name="profile" options={{ title: t('tabs.profile') }} />
    </Tabs>
  );
}
