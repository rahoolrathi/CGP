import { greetingKey } from './greeting';

const at = (hour: number) => new Date(2026, 10, 27, hour, 0, 0);

describe('greetingKey', () => {
  it.each([
    [0, 'home.greeting.morning'],
    [11, 'home.greeting.morning'],
    [12, 'home.greeting.afternoon'],
    [17, 'home.greeting.afternoon'],
    [18, 'home.greeting.evening'],
    [23, 'home.greeting.evening'],
  ])('at %i:00 returns %s', (hour, expected) => {
    expect(greetingKey(at(hour))).toBe(expected);
  });
});
