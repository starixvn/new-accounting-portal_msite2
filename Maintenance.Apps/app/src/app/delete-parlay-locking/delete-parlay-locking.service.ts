import { Injectable } from '@angular/core';
import * as _ from 'lodash';
import { HttpService } from '../shared/services/http.service';
import {
  DeleteParlayLockingRequest,
  MatchParlayLocking,
  ParlayLockingRequest,
} from './match-parlay-locking.model';

@Injectable()
export class MatchParlayLockingService {
  private readonly baseUrl = 'api/delete-temp-variable';

  constructor(private readonly httpService: HttpService) { }

  getAll(data: ParlayLockingRequest) {
    const params: ParlayLockingRequest = {
      matchId: _.isNumber(data.matchId) ? data.matchId : 0,
      timeRange: data.timeRange,
    };

    const url = `${this.baseUrl}/get-parlay-locking`;

    return this.httpService.post<MatchParlayLocking[]>(url, params);
  }

  deleteItem(data: DeleteParlayLockingRequest) {
    const url = `${this.baseUrl}/delete-parlay-locking`;

    return this.httpService.post(url, data);
  }
}
