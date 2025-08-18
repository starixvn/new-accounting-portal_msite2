import { isNumeric } from './index';

describe('isNumeric', () => {
  it('should return true for valid numbers', () => {
    expect(isNumeric(123)).toBeTrue();
    expect(isNumeric('123')).toBeTrue();
    expect(isNumeric(' 123 ')).toBeTrue();
    expect(isNumeric(0)).toBeTrue();
    expect(isNumeric('0')).toBeTrue();
  });

  it('should return false for invalid numbers', () => {
    expect(isNumeric('')).toBeFalse();
    expect(isNumeric(' ')).toBeFalse();
    expect(isNumeric('abc')).toBeFalse();
    expect(isNumeric(NaN)).toBeFalse();
    expect(isNumeric(undefined as any)).toBeFalse();
    expect(isNumeric(null as any)).toBeFalse();
  });
});
