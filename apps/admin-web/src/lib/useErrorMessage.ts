import { useTranslation } from 'react-i18next';
import { ApiError } from './apiClient';

/** Turns an API error code (for example "employee_id_exists") into a translated message. */
export function useErrorMessage() {
  const { t } = useTranslation();

  return (error: unknown) => {
    const code = error instanceof ApiError ? error.code : undefined;
    return code
      ? t(`errors.codes.${code}`, { defaultValue: t('errors.generic') })
      : t('errors.generic');
  };
}
