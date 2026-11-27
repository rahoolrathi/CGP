import { useState, type FormEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useErrorMessage } from '../../lib/useErrorMessage';
import { useAddDepartment } from './departmentsApi';

export function AddDepartmentForm() {
  const { t } = useTranslation();
  const errorMessage = useErrorMessage();
  const addDepartment = useAddDepartment();
  const [name, setName] = useState('');

  const submit = (event: FormEvent) => {
    event.preventDefault();
    addDepartment.mutate(name.trim(), { onSuccess: () => setName('') });
  };

  return (
    <form className="card form-row" onSubmit={submit}>
      <label className="field">
        <span>{t('admin.departments.name')}</span>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          minLength={2}
          maxLength={100}
        />
      </label>
      <button type="submit" className="button" disabled={addDepartment.isPending}>
        {addDepartment.isPending ? t('common.saving') : t('admin.departments.add')}
      </button>
      {addDepartment.isError && (
        <p role="alert" className="error">
          {errorMessage(addDepartment.error)}
        </p>
      )}
    </form>
  );
}
