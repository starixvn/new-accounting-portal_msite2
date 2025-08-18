import { TestBed } from '@angular/core/testing';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number.service';
import { HttpService } from '../shared/services/http.service';
import { Match } from './match.model';
import { of, Subject } from 'rxjs';

describe('DeleteTempVariableNumberService', () => {
  let service: DeleteTempVariableNumberService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['post']);
    TestBed.configureTestingModule({
      providers: [
        DeleteTempVariableNumberService,
        { provide: HttpService, useValue: spy },
      ],
    });
    service = TestBed.inject(DeleteTempVariableNumberService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpService.post with correct params on getAll', () => {
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(123);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-temp-variable',
      { matchId: 123 }
    );
  });

  it('should set matchId to 0 if not a number in getAll', () => {
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAll(undefined);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-temp-variable',
      { matchId: 0 }
    );
  });

  it('should call httpService.post with correct url and data on getDetail', () => {
    const match: Match = { matchId: 1, action: 'act' } as any;
    httpServiceSpy.post.and.returnValue(of(match));
    service.getDetail(match);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-detail-temp-variable',
      { ...match, ActionName: match.action }
    );
  });

  it('should call httpService.post and emit dataUpdate$ on deleteItem', (done) => {
    const match: Match = { matchId: 1 } as any;
    const response = { success: true };
    httpServiceSpy.post.and.returnValue(of(response));
    service.dataUpdate$.subscribe((res) => {
      expect(res).toEqual(response);
      done();
    });
    service.deleteItem(match);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/delete-temp-variable',
      { ...match }
    );
  });
});
