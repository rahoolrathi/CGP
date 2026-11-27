import { render, screen } from '@testing-library/react-native';
import '../../shared/i18n';
import { ThemeProvider } from '../../shared/theme/ThemeProvider';
import { HomeScreen } from './HomeScreen';

jest.mock(
  'react-native-safe-area-context',
  () => jest.requireActual('react-native-safe-area-context/jest/mock').default,
);

describe('HomeScreen', () => {
  it('shows the daily goal and placeholder copy', async () => {
    await render(
      <ThemeProvider>
        <HomeScreen />
      </ThemeProvider>,
    );

    expect(screen.getByText('Goal 10,000')).toBeOnTheScreen();
    expect(screen.getByText('Step tracking arrives in the next update.')).toBeOnTheScreen();
  });
});
