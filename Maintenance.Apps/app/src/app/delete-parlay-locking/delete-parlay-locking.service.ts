import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpService } from '../shared/services/http.service';
import {
  DeleteParlayLockingRequest,
  MatchParlayLocking,
  ParlayLockingRequest,
} from './match-parlay-locking.model';

@Injectable()
export class MatchParlayLockingService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(private httpService: HttpService) {}

  getAll(data: ParlayLockingRequest) {
    const url = `${this.baseUrl}/get-parlay-locking`;

    return this.httpService.post<MatchParlayLocking[]>(url, data);
  }

  deleteItem(data: DeleteParlayLockingRequest) {
    const url = `${this.baseUrl}/delete-parlay-locking`;

    return this.httpService.post(url, data);
  }
}
