import { minTouchTarget, radius, spacing } from '@cgb/ui-tokens';
import { router } from 'expo-router';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, FlatList, Pressable, StyleSheet, TextInput, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTheme } from '../../shared/theme/ThemeProvider';
import { AppText } from '../../shared/ui/AppText';
import { setCurrentEmployee } from './currentEmployee';
import { searchEmployees, type EmployeeSummary } from './employeesApi';

export function PickEmployeeScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const [search, setSearch] = useState('');
  const [employees, setEmployees] = useState<EmployeeSummary[]>([]);
  const [status, setStatus] = useState<'loading' | 'ready' | 'error'>('loading');

  useEffect(() => {
    let cancelled = false;
    const timer = setTimeout(() => {
      setStatus('loading');
      searchEmployees(search)
        .then((items) => {
          if (cancelled) return;
          setEmployees(items);
          setStatus('ready');
        })
        .catch(() => !cancelled && setStatus('error'));
    }, 300);

    return () => {
      cancelled = true;
      clearTimeout(timer);
    };
  }, [search]);

  const choose = (employee: EmployeeSummary) => {
    setCurrentEmployee(employee);
    router.replace('/');
  };

  return (
    <SafeAreaView style={[styles.root, { backgroundColor: colors.background }]}>
      <View style={styles.header}>
        <AppText variant="headline">{t('identity.title')}</AppText>
        <AppText tone="secondary">{t('identity.subtitle')}</AppText>
        <TextInput
          value={search}
          onChangeText={setSearch}
          placeholder={t('identity.search')}
          placeholderTextColor={colors.textSecondary}
          accessibilityLabel={t('identity.search')}
          autoCorrect={false}
          style={[
            styles.search,
            {
              borderColor: colors.border,
              backgroundColor: colors.surface,
              color: colors.textPrimary,
            },
          ]}
        />
      </View>

      {status === 'loading' && <ActivityIndicator color={colors.primary} />}
      {status === 'error' && (
        <AppText tone="secondary" style={styles.message}>
          {t('identity.loadError')}
        </AppText>
      )}

      <FlatList
        data={employees}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.list}
        renderItem={({ item }) => (
          <Pressable
            accessibilityRole="button"
            onPress={() => choose(item)}
            style={({ pressed }) => [
              styles.row,
              {
                backgroundColor: colors.surface,
                borderColor: colors.border,
                opacity: pressed ? 0.7 : 1,
              },
            ]}
          >
            <AppText variant="title">{item.fullName}</AppText>
            <AppText variant="caption" tone="secondary">
              {item.employeeId} · {item.departmentName}
            </AppText>
          </Pressable>
        )}
      />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1 },
  header: { padding: spacing.lg, gap: spacing.sm },
  search: {
    minHeight: minTouchTarget,
    borderWidth: 1,
    borderRadius: radius.md,
    paddingHorizontal: spacing.md,
    marginTop: spacing.sm,
    textAlign: 'auto',
  },
  message: { textAlign: 'center', padding: spacing.lg },
  list: { padding: spacing.lg, paddingTop: 0, gap: spacing.sm },
  row: {
    minHeight: minTouchTarget,
    padding: spacing.md,
    borderRadius: radius.md,
    borderWidth: StyleSheet.hairlineWidth,
    gap: 2,
  },
});
