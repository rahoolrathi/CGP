import { DashboardPage } from '../features/dashboard/DashboardPage';
import { AppShell } from './AppShell';
import { AppProviders } from './AppProviders';

export function App() {
  return (
    <AppProviders>
      <AppShell>
        <DashboardPage />
      </AppShell>
    </AppProviders>
  );
}
