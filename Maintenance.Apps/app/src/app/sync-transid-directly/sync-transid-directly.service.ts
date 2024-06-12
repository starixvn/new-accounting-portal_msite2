import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { HttpService } from '../shared/services/http.service';
import {
  DbInfo,
  GetBatchTransIdRequest,
  Result,
  SynMatch,
  UpdateBatchTransIdRequest,
  UpdateTransIdsRequest,
} from './sync-transid-directly.model';

@Injectable()
export class SyncTransIdService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(
    private httpService: HttpService,
    public dialogService: ConfirmDialogService
  ) {}

  public getDbInfo(): Observable<DbInfo> {
    return this.httpService.getData(`${this.baseUrl}/get-dbinfo`);
  }

  public updateTransId(request: UpdateTransIdsRequest): Observable<Result> {
    return this.httpService.post(
      `${this.baseUrl}/sync-transid-update`,
      request
    );
  }

  public getSynTicketForMatch(
    request: GetBatchTransIdRequest
  ): Observable<SynMatch[]> {
    return this.httpService.post(`${this.baseUrl}/get-sync-match`, request);
  }

  public updateBatchTransId(
    request: UpdateBatchTransIdRequest
  ): Observable<Result> {
    return this.httpService.post(
      `${this.baseUrl}/update-batch-transId`,
      request
    );
  }
}
