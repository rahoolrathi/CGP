import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDebouncedValue } from '../../lib/useDebouncedValue';
import { useErrorMessage } from '../../lib/useErrorMessage';
import { AddEmployeeForm } from './AddEmployeeForm';
import { DepartmentSelect } from './DepartmentSelect';
import { EmployeeRow } from './EmployeeRow';
import { ImportRosterForm } from './ImportRosterForm';
import { pageSize, useEmployees } from './employeesApi';

export function EmployeesPage() {
  const { t } = useTranslation();
  const errorMessage = useErrorMessage();
  const [search, setSearch] = useState('');
  const [departmentId, setDepartmentId] = useState('');
  const [page, setPage] = useState(1);
  const [rowError, setRowError] = useState<unknown>(null);

  const debouncedSearch = useDebouncedValue(search);
  const employees = useEmployees({ search: debouncedSearch, departmentId, page });
  const total = employees.data?.totalCount ?? 0;
  const pages = Math.max(1, Math.ceil(total / pageSize));

  return (
    <section className="page">
      <h1>{t('admin.employees.title')}</h1>
      <p className="muted">{t('admin.employees.subtitle')}</p>

      <AddEmployeeForm />
      <ImportRosterForm />

      <div className="toolbar">
        <input
          type="search"
          aria-label={t('admin.employees.search')}
          placeholder={t('admin.employees.search')}
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setPage(1);
          }}
        />
        <DepartmentSelect
          label={t('admin.employees.department')}
          emptyLabel={t('admin.employees.allDepartments')}
          value={departmentId}
          onChange={(id) => {
            setDepartmentId(id);
            setPage(1);
          }}
        />
        <span className="muted">{t('admin.employees.total', { count: total })}</span>
      </div>

      {rowError !== null && (
        <p role="alert" className="error">
          {errorMessage(rowError)}
        </p>
      )}
      {employees.isPending && <p className="muted">{t('common.loading')}</p>}
      {employees.isError && <p className="error">{errorMessage(employees.error)}</p>}
      {employees.data?.items.length === 0 && <p className="muted">{t('admin.employees.empty')}</p>}

      {employees.data && employees.data.items.length > 0 && (
        <table className="table">
          <thead>
            <tr>
              <th>{t('admin.employees.employeeId')}</th>
              <th>{t('admin.employees.fullName')}</th>
              <th>{t('admin.employees.workEmail')}</th>
              <th>{t('admin.employees.department')}</th>
              <th>
                <span className="visually-hidden">{t('common.delete')}</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {employees.data.items.map((employee) => (
              <EmployeeRow key={employee.id} employee={employee} onError={setRowError} />
            ))}
          </tbody>
        </table>
      )}

      <nav className="pager" aria-label={t('admin.employees.page', { page, pages })}>
        <button
          type="button"
          className="button button--ghost"
          disabled={page <= 1}
          onClick={() => setPage(page - 1)}
        >
          {t('common.previous')}
        </button>
        <span>{t('admin.employees.page', { page, pages })}</span>
        <button
          type="button"
          className="button button--ghost"
          disabled={page >= pages}
          onClick={() => setPage(page + 1)}
        >
          {t('common.next')}
        </button>
      </nav>
    </section>
  );
}
