import { TestBed } from '@angular/core/testing';
import { ControlSyncService } from './control-sync-service.service';
import { HttpService } from '../shared/services/http.service';
import { of } from 'rxjs';

class MockHttpService {
  post = jasmine.createSpy('post');
}

describe('ControlSyncService', () => {
  let service: ControlSyncService;
  let mockHttpService: MockHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ControlSyncService,
        { provide: HttpService, useClass: MockHttpService },
      ],
    });
    service = TestBed.inject(ControlSyncService);
    mockHttpService = TestBed.inject(HttpService) as any;
    mockHttpService.post.calls.reset();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpService.post on updateStatus', () => {
    mockHttpService.post.and.returnValue(of({}));
    const status = true;
    const cacheKey = 'key';
    service.updateStatus(status, cacheKey);
    expect(mockHttpService.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/update-status',
      { status, cacheKey }
    );
  });

  it('should call httpService.post on getInfoService', () => {
    mockHttpService.post.and.returnValue(of([]));
    service.getInfoService();
    expect(mockHttpService.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-info-service'
    );
  });
});
