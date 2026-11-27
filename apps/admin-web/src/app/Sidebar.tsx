import { useTranslation } from 'react-i18next';
import { routeHref, routes, type Route } from './routes';

export function Sidebar({ current }: { current: Route }) {
  const { t } = useTranslation();

  return (
    <nav className="sidebar" aria-label={t('admin.nav.label')}>
      {routes.map((route) => (
        <a
          key={route}
          href={routeHref(route)}
          className="sidebar__link"
          aria-current={route === current ? 'page' : undefined}
        >
          {t(`admin.nav.${route}`)}
        </a>
      ))}
    </nav>
  );
}
