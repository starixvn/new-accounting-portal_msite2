import { Injectable } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { HttpService } from '../shared/services/http.service';
import {
  DetailSettlementErrorLog,
  SettlementErrorLog,
} from './sync-settlement-log.model';

@Injectable()
export class SettlementErrorLogService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(
    private httpService: HttpService,
    public dialogService: ConfirmDialogService
  ) {}

  public getAll(matchId: number, typeSettle: string) {
    const url = `${this.baseUrl}/get-settlement-error-logs`;
    const body = {
      matchId,
      mode: typeSettle,
    };
    return this.httpService.post<SettlementErrorLog[]>(url, body);
  }

  public getDetails(item: SettlementErrorLog, mode: string) {
    const url = `${this.baseUrl}/get-settlement-error-log-detail`;
    const body = {
      sessionId: item.sessionId,
      siteDBGroupId: item.siteDBGroupId,
      dbServer: item.dbServer,
      mode,
    };
    return this.httpService.post<DetailSettlementErrorLog[]>(url, body);
  }

  public deleteInLogSettlementLock(item: SettlementErrorLog, mode: string) {
    const url = `${this.baseUrl}/delete-settlement-lock-error-log`;
    const body = {
      sessionId: item.sessionId,
      siteDBGroupId: item.siteDBGroupId,
      dbServer: item.dbServer,
      mode,
    };
    return this.httpService.post<number>(url, body);
  }

  public deleteInSettlementProcessing(
    item: SettlementErrorLog,
    action: string,
    mode: string
  ) {
    const url = `${this.baseUrl}/delete-settlement-processing-error-log`;
    const body = {
      sessionId: item.sessionId,
      siteDBGroupId: item.siteDBGroupId,
      action: action,
      dbServer: item.dbServer,
      mode,
    };
    return this.httpService.post<number>(url, body);
  }

  public updateInSSBSettlementLogs(item: SettlementErrorLog) {
    const url = `${this.baseUrl}/update-ssb-settlement-error-log`;
    const body = {
      sessionId: item.sessionId,
      siteDBGroupId: item.siteDBGroupId,
      dbServer: item.dbServer,
      startTime: item.createdTime,
      messageId: item.messageId,
    };
    return this.httpService.post<number>(url, body);
  }
}
