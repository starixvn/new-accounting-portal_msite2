import { HttpService } from './http.service';
import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { BlockUIService } from './block-ui.service';
import { SnackbarService } from './snackbar.service';
import { environment } from 'src/environments/environment';
import { of, throwError } from 'rxjs';

describe('HttpService', () => {
  let service: HttpService;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;
  let blockUISpy: jasmine.SpyObj<BlockUIService>;
  let snackbarSpy: jasmine.SpyObj<SnackbarService>;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get', 'post']);
    blockUISpy = jasmine.createSpyObj('BlockUIService', [
      'startBlock',
      'stopBlock',
    ]);
    snackbarSpy = jasmine.createSpyObj('SnackbarService', [
      'openErrorSnackBar',
    ]);
    service = new HttpService(httpClientSpy, blockUISpy, snackbarSpy);
  });

  it('should call get with correct url and params, and stop block on response', (done) => {
    const testData = { foo: 'bar' };
    httpClientSpy.get.and.returnValue(of(testData));
    service.getData('test', new HttpParams()).subscribe((data) => {
      expect(data).toEqual(testData);
      expect(blockUISpy.startBlock).toHaveBeenCalled();
      expect(blockUISpy.stopBlock).toHaveBeenCalled();
      done();
    });
    expect(httpClientSpy.get).toHaveBeenCalledWith(
      environment.baseUrl + 'test',
      { params: jasmine.any(HttpParams) }
    );
  });

  it('should call post with correct url, item, and params, and stop block on response', (done) => {
    const testData = { foo: 'bar' };
    httpClientSpy.post.and.returnValue(of(testData));
    service.post('test', { foo: 'bar' }, new HttpParams()).subscribe((data) => {
      expect(data).toEqual(testData);
      expect(blockUISpy.startBlock).toHaveBeenCalled();
      expect(blockUISpy.stopBlock).toHaveBeenCalled();
      done();
    });
    expect(httpClientSpy.post).toHaveBeenCalledWith(
      environment.baseUrl + 'test',
      { foo: 'bar' },
      { params: jasmine.any(HttpParams) }
    );
  });

  it('should handle error with BAD_REQUEST_STATUS and call snackbar with errors', (done) => {
    const errorResponse = new HttpErrorResponse({
      status: 400,
      error: { errors: { msg: 'bad request' } },
      statusText: 'Bad Request',
    });
    httpClientSpy.get.and.returnValue(throwError(() => errorResponse));
    service.getData('test').subscribe({
      error: () => {
        expect(blockUISpy.stopBlock).toHaveBeenCalled();
        expect(snackbarSpy.openErrorSnackBar).toHaveBeenCalledWith(
          JSON.stringify({ msg: 'bad request' })
        );
        done();
      },
    });
  });

  it('should handle error with other status and call snackbar with statusText', (done) => {
    const errorResponse = new HttpErrorResponse({
      status: 500,
      error: {},
      statusText: 'Server Error',
    });
    httpClientSpy.get.and.returnValue(throwError(() => errorResponse));
    service.getData('test').subscribe({
      error: () => {
        expect(blockUISpy.stopBlock).toHaveBeenCalled();
        expect(snackbarSpy.openErrorSnackBar).toHaveBeenCalledWith(
          'Server Error'
        );
        done();
      },
    });
  });
});
