import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render, screen } from '@testing-library/react';
import type { ReactNode } from 'react';
import '../../lib/i18n';
import { DashboardPage } from './DashboardPage';

function renderWithClient(ui: ReactNode) {
  const client = new QueryClient({ defaultOptions: { queries: { retry: false } } });
  return render(<QueryClientProvider client={client}>{ui}</QueryClientProvider>);
}

function mockFetch(response: Response) {
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue(response));
}

afterEach(() => vi.unstubAllGlobals());

describe('DashboardPage', () => {
  it('shows the API as healthy when system info loads', async () => {
    mockFetch(
      Response.json({
        version: '0.1.0',
        serverTimeUtc: '2026-11-27T08:00:00Z',
        qatarDate: '2026-11-27',
      }),
    );

    renderWithClient(<DashboardPage />);

    expect(await screen.findByText('Healthy')).toBeInTheDocument();
    expect(screen.getByText('v0.1.0')).toBeInTheDocument();
  });

  it('shows the API as unavailable when the request fails', async () => {
    mockFetch(Response.json({ code: 'server_error' }, { status: 503 }));

    renderWithClient(<DashboardPage />);

    expect(await screen.findByText('Unavailable')).toBeInTheDocument();
  });

  it('rejects responses that do not match the contract', async () => {
    mockFetch(Response.json({ unexpected: true }));

    renderWithClient(<DashboardPage />);

    expect(await screen.findByText('Unavailable')).toBeInTheDocument();
  });
});
