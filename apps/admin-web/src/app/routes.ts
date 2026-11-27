import { useSyncExternalStore } from 'react';

export const routes = ['dashboard', 'employees', 'departments'] as const;
export type Route = (typeof routes)[number];

function subscribe(onChange: () => void) {
  window.addEventListener('hashchange', onChange);
  return () => window.removeEventListener('hashchange', onChange);
}

function currentRoute(): Route {
  const hash = window.location.hash.replace(/^#\/?/, '');
  return routes.find((route) => route === hash) ?? 'dashboard';
}

/** Simple hash routing (#/employees) so the app needs no router library yet. */
export function useRoute() {
  return useSyncExternalStore(subscribe, currentRoute);
}

export function routeHref(route: Route) {
  return `#/${route}`;
}
