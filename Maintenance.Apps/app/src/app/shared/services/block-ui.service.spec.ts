import { BlockUIService } from './block-ui.service';

describe('BlockUIService', () => {
  let service: BlockUIService;

  beforeEach(() => {
    service = new BlockUIService();
  });

  it('should create blockUIEvent as EventEmitter', () => {
    expect(service.blockUIEvent).toBeDefined();
    expect(service.blockUIEvent.emit).toBeDefined();
  });

  it('should emit true when startBlock is called', () => {
    spyOn(service.blockUIEvent, 'emit');
    service.startBlock();
    expect(service.blockUIEvent.emit).toHaveBeenCalledWith(true);
  });

  it('should emit false when stopBlock is called', () => {
    spyOn(service.blockUIEvent, 'emit');
    service.stopBlock();
    expect(service.blockUIEvent.emit).toHaveBeenCalledWith(false);
  });
});
