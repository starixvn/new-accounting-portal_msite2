import { TestBed } from '@angular/core/testing';
import { MatchParlayLockingService } from './delete-parlay-locking.service';
import { HttpService } from '../shared/services/http.service';
import {
  ParlayLockingRequest,
  DeleteParlayLockingRequest,
  MatchParlayLocking,
} from './match-parlay-locking.model';
import { of } from 'rxjs';

describe('MatchParlayLockingService', () => {
  let service: MatchParlayLockingService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['post']);
    TestBed.configureTestingModule({
      providers: [
        MatchParlayLockingService,
        { provide: HttpService, useValue: spy },
      ],
    });
    service = TestBed.inject(MatchParlayLockingService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpService.post with correct params on getAll', () => {
    const data: ParlayLockingRequest = { matchId: 123, timeRange: 12 };
    const expectedParams = { matchId: 123, timeRange: 12 };
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(data);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-parlay-locking',
      expectedParams
    );
  });

  it('should set matchId to 0 if not a number in getAll', () => {
    const data: ParlayLockingRequest = { matchId: undefined, timeRange: 1 };
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(data);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-parlay-locking',
      { matchId: 0, timeRange: 1 }
    );
  });

  it('should call httpService.post with correct url and data on deleteItem', () => {
    const req: DeleteParlayLockingRequest = { matchId: 123 } as any;
    httpServiceSpy.post.and.returnValue(of({}));
    service.deleteItem(req);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/delete-parlay-locking',
      req
    );
  });
});
