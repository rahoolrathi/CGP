import { useState, type FormEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useErrorMessage } from '../../lib/useErrorMessage';
import { DepartmentSelect } from './DepartmentSelect';
import { useAddEmployee, type NewEmployee } from './employeesApi';

const emptyEmployee: NewEmployee = {
  employeeId: '',
  fullName: '',
  workEmail: '',
  departmentId: '',
};

export function AddEmployeeForm() {
  const { t } = useTranslation();
  const errorMessage = useErrorMessage();
  const addEmployee = useAddEmployee();
  const [employee, setEmployee] = useState(emptyEmployee);

  const update = (field: keyof NewEmployee) => (value: string) =>
    setEmployee((current) => ({ ...current, [field]: value }));

  const submit = (event: FormEvent) => {
    event.preventDefault();
    addEmployee.mutate(employee, { onSuccess: () => setEmployee(emptyEmployee) });
  };

  return (
    <form className="card form-grid" onSubmit={submit}>
      <label className="field">
        <span>{t('admin.employees.employeeId')}</span>
        <input
          value={employee.employeeId}
          onChange={(e) => update('employeeId')(e.target.value)}
          required
          maxLength={20}
          pattern="[A-Za-z0-9\-]+"
        />
      </label>
      <label className="field">
        <span>{t('admin.employees.fullName')}</span>
        <input
          value={employee.fullName}
          onChange={(e) => update('fullName')(e.target.value)}
          required
          minLength={2}
          maxLength={200}
        />
      </label>
      <label className="field">
        <span>{t('admin.employees.workEmail')}</span>
        <input
          type="email"
          value={employee.workEmail}
          onChange={(e) => update('workEmail')(e.target.value)}
          required
          maxLength={254}
        />
      </label>
      <label className="field">
        <span>{t('admin.employees.department')}</span>
        <DepartmentSelect
          label={t('admin.employees.department')}
          emptyLabel={t('admin.employees.chooseDepartment')}
          value={employee.departmentId}
          onChange={update('departmentId')}
          required
        />
      </label>
      <button type="submit" className="button" disabled={addEmployee.isPending}>
        {addEmployee.isPending ? t('common.saving') : t('admin.employees.add')}
      </button>
      {addEmployee.isError && (
        <p role="alert" className="error">
          {errorMessage(addEmployee.error)}
        </p>
      )}
    </form>
  );
}
