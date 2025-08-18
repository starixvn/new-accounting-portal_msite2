import { Injectable } from '@angular/core';
import * as _ from 'lodash';
import { HttpService } from '../shared/services/http.service';
import {
  DeleteSettlementLog,
  MatchSettlementLock,
  UpdateSettlementLog,
} from './match-settlement-lock.model';

@Injectable()
export class MatchSettlementLockService {
  private readonly baseUrl = 'api/delete-temp-variable';

  constructor(private readonly httpService: HttpService) {}

  getAll(matchId?: number, mode?: string) {
    matchId = _.isNumber(matchId) ? matchId : 0;
    const url = `${this.baseUrl}/get-settlement-lock`;

    return this.httpService.post<MatchSettlementLock[]>(url, {
      matchId,
      mode,
    });
  }

  deleteItem(data: DeleteSettlementLog, mode: string) {
    const url = `${this.baseUrl}/delete-settlement-lock`;

    return this.httpService.post(url, { ...data, mode });
  }

  updateItem(item: MatchSettlementLock, mode: string) {
    const params: UpdateSettlementLog = {
      sessionId: item.sessionId,
      createdTime: item.createdTime,
      dbServer: item.dbServer,
      messageId: item.messageId,
    };

    const url = `${this.baseUrl}/update-settlement-lock`;

    return this.httpService.post(url, { ...params, mode });
  }
}
