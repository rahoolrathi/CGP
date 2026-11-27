import { useTranslation } from 'react-i18next';
import { useErrorMessage } from '../../lib/useErrorMessage';
import { AddDepartmentForm } from './AddDepartmentForm';
import { useDeleteDepartment, useDepartments, type Department } from './departmentsApi';

export function DepartmentsPage() {
  const { t, i18n } = useTranslation();
  const errorMessage = useErrorMessage();
  const departments = useDepartments();
  const deleteDepartment = useDeleteDepartment();

  const remove = (department: Department) => {
    if (window.confirm(t('admin.departments.confirmDelete', { name: department.name }))) {
      deleteDepartment.mutate(department.id);
    }
  };

  return (
    <section className="page">
      <h1>{t('admin.departments.title')}</h1>
      <p className="muted">{t('admin.departments.subtitle')}</p>

      <AddDepartmentForm />

      {deleteDepartment.isError && (
        <p role="alert" className="error">
          {errorMessage(deleteDepartment.error)}
        </p>
      )}

      {departments.isPending && <p className="muted">{t('common.loading')}</p>}
      {departments.isError && <p className="error">{errorMessage(departments.error)}</p>}
      {departments.data?.length === 0 && <p className="muted">{t('admin.departments.empty')}</p>}

      {departments.data && departments.data.length > 0 && (
        <table className="table">
          <thead>
            <tr>
              <th>{t('admin.departments.name')}</th>
              <th>{t('admin.departments.employees')}</th>
              <th>{t('admin.departments.created')}</th>
              <th>
                <span className="visually-hidden">{t('common.delete')}</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {departments.data.map((department) => (
              <tr key={department.id}>
                <td>{department.name}</td>
                <td>{department.employeeCount.toLocaleString(i18n.language)}</td>
                <td>{new Date(department.createdAt).toLocaleDateString(i18n.language)}</td>
                <td className="table__actions">
                  <button
                    type="button"
                    className="button button--danger"
                    onClick={() => remove(department)}
                    disabled={deleteDepartment.isPending}
                  >
                    {t('common.delete')}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
