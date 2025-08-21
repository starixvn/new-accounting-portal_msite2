import { FileService } from './file.service';

describe('FileService', () => {
  let service: FileService;

  beforeEach(() => {
    service = new FileService();
  });

  it('should create an anchor and trigger download', () => {
    const createElementSpy = spyOn(document, 'createElement').and.callFake(
      () => {
        return {
          set download(val: string) {},
          set href(val: string) {},
          click: jasmine.createSpy('click'),
        } as any;
      }
    );
    const urlSpy = spyOn(window.URL, 'createObjectURL').and.returnValue(
      'blob:url'
    );
    const data = 'test data';
    const filename = 'test.txt';

    service.saveTextAsFile(data, filename);

    expect(createElementSpy).toHaveBeenCalledWith('a');
    expect(urlSpy).toHaveBeenCalled();
  });
});
