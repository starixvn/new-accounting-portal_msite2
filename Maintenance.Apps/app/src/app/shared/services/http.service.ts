import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SnackbarService } from './snackbar.service';

@Injectable()
export class HttpService {
  constructor(
    private httpClient: HttpClient,
    private snackbarService: SnackbarService
  ) {}

  public getData<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient
      .get<T>(this.getUrl(url), { params })
      .pipe(catchError(this.handleError));
  }

  public post<T>(url: string, item?: any, params?: HttpParams): Observable<T> {
    return this.httpClient
      .post<T>(this.getUrl(url), item, { params })
      .pipe(catchError(this.handleError));
  }

  private getUrl(url: string) {
    return environment.baseUrl + url;
  }

  private handleError(error: HttpErrorResponse) {
    this.snackbarService.openErrorSnackBar(error.message);
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
