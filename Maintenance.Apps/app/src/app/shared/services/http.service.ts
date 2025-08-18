import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SnackbarService } from './snackbar.service';
import { BlockUIService } from './block-ui.service';

// HTTP status code constants
const BAD_REQUEST_STATUS = 400;

@Injectable()
export class HttpService {
  constructor(
    private readonly httpClient: HttpClient,
    private readonly blockUIService: BlockUIService,
    private readonly snackbarService: SnackbarService
  ) { }

  public getData<T>(url: string, params?: HttpParams): Observable<T> {
    this.blockUIService.startBlock();
    return this.httpClient.get<T>(this.getUrl(url), { params }).pipe(
      map((response) => this.parseResponse(response)),
      catchError(this.handleError)
    );
  }

  public post<T>(url: string, item?, params?: HttpParams): Observable<T> {
    this.blockUIService.startBlock();
    return this.httpClient.post<T>(this.getUrl(url), item, { params }).pipe(
      map((response) => this.parseResponse(response)),
      catchError(this.handleError)
    );
  }

  private getUrl(url: string) {
    return environment.baseUrl + url;
  }

  private parseResponse(response) {
    this.blockUIService.stopBlock();
    return response;
  }

  private readonly handleError = (error: HttpErrorResponse) => {
    this.blockUIService.stopBlock();

    if (error.status === BAD_REQUEST_STATUS && error.error.errors) {
      this.snackbarService.openErrorSnackBar(JSON.stringify(error.error.errors));
    } else {
      this.snackbarService.openErrorSnackBar(error.statusText);
    }
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  };
}
