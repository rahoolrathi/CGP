import { useRef, type FormEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useErrorMessage } from '../../lib/useErrorMessage';
import { useImportRoster } from './employeesApi';

export function ImportRosterForm() {
  const { t } = useTranslation();
  const errorMessage = useErrorMessage();
  const importRoster = useImportRoster();
  const fileInput = useRef<HTMLInputElement>(null);

  const submit = (event: FormEvent) => {
    event.preventDefault();
    const file = fileInput.current?.files?.[0];
    if (!file) return;
    importRoster.mutate(file, {
      onSuccess: () => {
        if (fileInput.current) fileInput.current.value = '';
      },
    });
  };

  const result = importRoster.data;

  return (
    <form className="card" onSubmit={submit}>
      <h2>{t('admin.employees.import.title')}</h2>
      <p className="muted">{t('admin.employees.import.hint')}</p>
      <div className="form-row">
        <label className="field">
          <span className="visually-hidden">{t('admin.employees.import.choose')}</span>
          <input ref={fileInput} type="file" accept=".csv,text/csv" required />
        </label>
        <button type="submit" className="button" disabled={importRoster.isPending}>
          {importRoster.isPending ? t('common.saving') : t('admin.employees.import.upload')}
        </button>
      </div>

      {importRoster.isError && (
        <p role="alert" className="error">
          {errorMessage(importRoster.error)}
        </p>
      )}

      {result && (
        <div aria-live="polite">
          <p data-state="ok">
            {t('admin.employees.import.result', {
              added: result.added,
              skipped: result.skipped,
              departments: result.departmentsCreated,
            })}
          </p>
          {result.errors.length > 0 && (
            <ul className="error-list">
              {result.errors.map((error) => (
                <li key={error.row}>
                  {t('admin.employees.import.rowError', {
                    row: error.row,
                    message: t(`errors.codes.${error.code}`, { defaultValue: error.code }),
                  })}
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </form>
  );
}
