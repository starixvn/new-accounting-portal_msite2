import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpService } from '../shared/services/http.service';
import {
  DeleteSettlementLog,
  MatchSettlementLock,
  UpdateSettlementLog,
} from './match-settlement-lock.model';

@Injectable()
export class MatchSettlementLockService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(private httpService: HttpService) {}

  getAll(matchId: string) {
    const url = `${this.baseUrl}/get-settlement-lock`;

    return this.httpService.post<MatchSettlementLock[]>(url, {
      matchId,
    });
  }

  deleteItem(data: DeleteSettlementLog) {
    const url = `${this.baseUrl}/delete-settlement-lock`;

    return this.httpService.post(url, data);
  }

  updateItem(data: UpdateSettlementLog) {
    const url = `${this.baseUrl}/update-settlement-lock`;

    return this.httpService.post(url, data);
  }
}
