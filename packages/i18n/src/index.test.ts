import { describe, expect, it } from 'vitest';
import { isRtl, resolveLanguage, resources } from './index';

function keyPaths(node: unknown, prefix = ''): string[] {
  if (typeof node !== 'object' || node === null) return [prefix];
  return Object.entries(node).flatMap(([key, value]) =>
    keyPaths(value, prefix ? `${prefix}.${key}` : key),
  );
}

describe('i18n', () => {
  it('has the same keys in every language', () => {
    expect(keyPaths(resources.ar.translation).sort()).toEqual(
      keyPaths(resources.en.translation).sort(),
    );
  });

  it('marks Arabic as right-to-left', () => {
    expect(isRtl('ar')).toBe(true);
    expect(isRtl('en')).toBe(false);
  });

  it.each([
    ['ar-QA', 'ar'],
    ['en_US', 'en'],
    ['fr', 'en'],
    [undefined, 'en'],
  ])('resolves %s to %s', (input, expected) => {
    expect(resolveLanguage(input)).toBe(expected);
  });
});
