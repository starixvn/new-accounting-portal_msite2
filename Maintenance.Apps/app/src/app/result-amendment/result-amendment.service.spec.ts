import { TestBed } from '@angular/core/testing';
import { ResultAmendmentService } from './result-amendment.service';
import { HttpService } from '../shared/services/http.service';
import { AmendmentResultUpdateRequest, ResultAmendment } from './type';
import { of } from 'rxjs';

describe('ResultAmendmentService', () => {
  let service: ResultAmendmentService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['post']);
    TestBed.configureTestingModule({
      providers: [
        ResultAmendmentService,
        { provide: HttpService, useValue: spy },
      ],
    });
    service = TestBed.inject(ResultAmendmentService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpService.post with correct url and request on updateStatus', () => {
    const req: AmendmentResultUpdateRequest = {
      results: [{ selected: true }] as ResultAmendment[],
    };
    httpServiceSpy.post.and.returnValue(of({}));
    service.updateStatus(req);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/update-amendment-result',
      req
    );
  });

  it('should call httpService.post with correct url and request on getAmenentResults', () => {
    const req = { matchId: 1, statusId: 2 };
    httpServiceSpy.post.and.returnValue(of([]));
    service.getAmenentResults(req);
    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      'api/delete-temp-variable/get-amendment-results',
      req
    );
  });
});
