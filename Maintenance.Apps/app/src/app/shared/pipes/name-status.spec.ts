import { NameStatus } from './name-status';

describe('NameStatus Pipe', () => {
  let pipe: NameStatus;

  beforeEach(() => {
    pipe = new NameStatus();
  });

  it('should return timeout message for status 3', () => {
    expect(pipe.transform(3)).toBe('[3] Failed: operation timed out.');
  });

  it('should return settlement failure message for status 99', () => {
    expect(pipe.transform(99)).toBe(
      '[99] Failed: cannot settle all tickets in one action.'
    );
  });

  it('should return value as string for other statuses', () => {
    expect(pipe.transform(1)).toBe('1');
    expect(pipe.transform(0)).toBe('0');
    expect(pipe.transform(-1)).toBe('-1');
    expect(pipe.transform(100)).toBe('100');
  });
});
