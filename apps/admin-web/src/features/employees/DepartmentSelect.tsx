import { useTranslation } from 'react-i18next';
import { useDepartments } from '../departments/departmentsApi';

interface DepartmentSelectProps {
  value: string;
  onChange: (departmentId: string) => void;
  /** Text for the empty option, for example "All departments". */
  emptyLabel: string;
  label: string;
  required?: boolean;
  disabled?: boolean;
}

export function DepartmentSelect({
  value,
  onChange,
  emptyLabel,
  label,
  required,
  disabled,
}: DepartmentSelectProps) {
  const { t } = useTranslation();
  const departments = useDepartments();

  return (
    <select
      aria-label={label}
      value={value}
      onChange={(e) => onChange(e.target.value)}
      required={required}
      disabled={disabled ?? departments.isPending}
    >
      <option value="">{departments.isPending ? t('common.loading') : emptyLabel}</option>
      {departments.data?.map((department) => (
        <option key={department.id} value={department.id}>
          {department.name}
        </option>
      ))}
    </select>
  );
}
