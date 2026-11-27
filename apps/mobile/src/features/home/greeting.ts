export type GreetingKey =
  'home.greeting.morning' | 'home.greeting.afternoon' | 'home.greeting.evening';

export function greetingKey(date: Date): GreetingKey {
  const hour = date.getHours();
  if (hour < 12) return 'home.greeting.morning';
  if (hour < 18) return 'home.greeting.afternoon';
  return 'home.greeting.evening';
}
