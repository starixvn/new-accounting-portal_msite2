import { TestBed } from '@angular/core/testing';
import { MatchSettlementLockService } from './match-settlement-lock.service';
import { HttpService } from '../shared/services/http.service';
import {
  MatchSettlementLock,
  DeleteSettlementLog,
  UpdateSettlementLog,
} from './match-settlement-lock.model';
import { of } from 'rxjs';

describe('MatchSettlementLockService', () => {
  let service: MatchSettlementLockService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['post']);
    TestBed.configureTestingModule({
      providers: [
        MatchSettlementLockService,
        { provide: HttpService, useValue: spy },
      ],
    });
    service = TestBed.inject(MatchSettlementLockService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpService.post with correct params on getAll', () => {
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(123, 'single');
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-settlement-lock',
      { matchId: 123, mode: 'single' }
    );
  });

  it('should set matchId to 0 if not a number in getAll', () => {
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(undefined, 'single');
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-settlement-lock',
      { matchId: 0, mode: 'single' }
    );
  });

  it('should call httpService.post with correct url and data on deleteItem', () => {
    const req: DeleteSettlementLog = {
      dbServer: 'DB1',
      sessionId: 2,
      isSettleLock: true,
    };
    httpServiceSpy.post.and.returnValue(of({}));
    service.deleteItem(req, 'single');
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/delete-settlement-lock',
      { ...req, mode: 'single' }
    );
  });

  it('should call httpService.post with correct url and params on updateItem', () => {
    const item: MatchSettlementLock = {
      matchId: 1,
      sessionId: 2,
      createdTime: '2025-08-18',
      homeTeam: 'A',
      awayTeam: 'B',
      totalSettlementProcessing: 10,
      dbServer: 'DB1',
      serverName: 'S1',
      errorMessage: '',
      messageId: 'msg1',
      isReplicated: false,
    };
    httpServiceSpy.post.and.returnValue(of({}));
    service.updateItem(item, 'single');
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/update-settlement-lock',
      {
        sessionId: item.sessionId,
        createdTime: item.createdTime,
        dbServer: item.dbServer,
        messageId: item.messageId,
        mode: 'single',
      }
    );
  });
});
