import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class HttpService {
  constructor(private httpClient: HttpClient) {}

  public getData<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient
      .get<T>(this.getUrl(url), { params })
      .pipe(catchError(this.handleError));
  }

  public post<T>(url: string, item: T): Observable<T> {
    return this.httpClient
      .post<T>(this.getUrl(url), item)
      .pipe(catchError(this.handleError));
  }

  private getUrl(url: string) {
    return environment.baseUrl + url;
  }

  private handleError(error: HttpErrorResponse) {
    console.log(error);
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
