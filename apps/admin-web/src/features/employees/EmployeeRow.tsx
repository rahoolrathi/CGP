import { useTranslation } from 'react-i18next';
import { DepartmentSelect } from './DepartmentSelect';
import { useChangeDepartment, useDeleteEmployee, type Employee } from './employeesApi';

interface EmployeeRowProps {
  employee: Employee;
  onError: (error: unknown) => void;
}

export function EmployeeRow({ employee, onError }: EmployeeRowProps) {
  const { t } = useTranslation();
  const changeDepartment = useChangeDepartment();
  const deleteEmployee = useDeleteEmployee();
  const busy = changeDepartment.isPending || deleteEmployee.isPending;

  const move = (departmentId: string) => {
    if (departmentId && departmentId !== employee.departmentId) {
      changeDepartment.mutate({ id: employee.id, departmentId }, { onError });
    }
  };

  const remove = () => {
    if (
      window.confirm(
        t('admin.employees.confirmDelete', { name: employee.fullName, id: employee.employeeId }),
      )
    ) {
      deleteEmployee.mutate(employee.id, { onError });
    }
  };

  return (
    <tr>
      <td>{employee.employeeId}</td>
      <td>{employee.fullName}</td>
      <td dir="ltr">{employee.workEmail}</td>
      <td>
        <DepartmentSelect
          label={t('admin.employees.department')}
          emptyLabel={employee.departmentName}
          value={employee.departmentId}
          onChange={move}
          disabled={busy}
        />
      </td>
      <td className="table__actions">
        <button type="button" className="button button--danger" onClick={remove} disabled={busy}>
          {t('common.delete')}
        </button>
      </td>
    </tr>
  );
}
