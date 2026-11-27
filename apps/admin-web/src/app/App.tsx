import type { ComponentType } from 'react';
import { DashboardPage } from '../features/dashboard/DashboardPage';
import { DepartmentsPage } from '../features/departments/DepartmentsPage';
import { EmployeesPage } from '../features/employees/EmployeesPage';
import { AppProviders } from './AppProviders';
import { AppShell } from './AppShell';
import { useRoute, type Route } from './routes';

const pages: Record<Route, ComponentType> = {
  dashboard: DashboardPage,
  employees: EmployeesPage,
  departments: DepartmentsPage,
};

export function App() {
  const route = useRoute();
  const Page = pages[route];

  return (
    <AppProviders>
      <AppShell route={route}>
        <Page />
      </AppShell>
    </AppProviders>
  );
}
