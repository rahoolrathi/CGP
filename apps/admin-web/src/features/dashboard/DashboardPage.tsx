import { useTranslation } from 'react-i18next';
import { useSystemInfo } from './useSystemInfo';

export function DashboardPage() {
  const { t } = useTranslation();
  const { data, isPending, isError } = useSystemInfo();

  const status = isPending
    ? t('common.loading')
    : isError
      ? t('admin.apiStatus.unavailable')
      : t('admin.apiStatus.healthy');

  return (
    <section className="page">
      <h1>{t('admin.dashboard.title')}</h1>
      <p className="muted">{t('admin.dashboard.subtitle')}</p>

      <div className="card" aria-live="polite">
        <span className="muted">{t('admin.apiStatus.label')}</span>
        <strong data-state={isError ? 'error' : isPending ? 'pending' : 'ok'}>{status}</strong>
        {data && <span className="muted">v{data.version}</span>}
      </div>
    </section>
  );
}
