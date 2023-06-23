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

@Injectable()
export class HttpService {
    constructor(
        private httpClient: HttpClient,
        private blockUIService: BlockUIService,
        private snackbarService: SnackbarService
    ) { }

    public getData<T>(url: string, params?: HttpParams): Observable<T> {
        this.blockUIService.startBlock();
        return this.httpClient.get<T>(this.getUrl(url), { params }).pipe(
            map((response) => this.parseResponse(response)),
            catchError(this.handleError)
        );
    }

    public post<T>(url: string, item?: any, params?: HttpParams): Observable<T> {
        this.blockUIService.startBlock();
        return this.httpClient.post<T>(this.getUrl(url), item, { params }).pipe(
            map((response) => this.parseResponse(response)),
            catchError(this.handleError)
        );
    }

    private getUrl(url: string) {
        return environment.baseUrl + url;
    }

    private parseResponse(response: any) {
        this.blockUIService.stopBlock();
        return response;
    }

    private handleError = (error: HttpErrorResponse) => {
        this.blockUIService.stopBlock();
        this.snackbarService.openErrorSnackBar(error.message);
        return throwError(
            () => new Error('Something bad happened; please try again later.')
        );
    };
}
