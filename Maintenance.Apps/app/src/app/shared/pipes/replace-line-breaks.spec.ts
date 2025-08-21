import { ReplaceLineBreaks } from './replace-line-breaks';

describe('ReplaceLineBreaks Pipe', () => {
  let pipe: ReplaceLineBreaks;

  beforeEach(() => {
    pipe = new ReplaceLineBreaks();
  });

  it('should return empty string for empty input', () => {
    expect(pipe.transform('')).toBe('');
    expect(pipe.transform(null as any)).toBe('');
    expect(pipe.transform(undefined as any)).toBe('');
  });

  it('should replace \n with <br/>', () => {
    expect(pipe.transform('line1\nline2')).toBe('line1<br/>line2');
    expect(pipe.transform('a\nb\nc')).toBe('a<br/>b<br/>c');
  });

  it('should return original string if no line breaks', () => {
    expect(pipe.transform('no line break')).toBe('no line break');
  });
});
