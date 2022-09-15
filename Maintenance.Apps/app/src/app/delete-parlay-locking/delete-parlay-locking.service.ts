import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpService } from '../shared/services/http.service';
import { MatchParlayLocking } from './match-parlay-locking.model';

@Injectable()
export class MatchParlayLockingService {
  private baseUrl = 'api/delete-temp-variable';

  private dataUpdateSource = new Subject();
  public dataUpdate$ = this.dataUpdateSource.asObservable();

  constructor(private httpService: HttpService) {}

  getAll(data: any) {
    const url = `${this.baseUrl}/get-parlay-locking`;

    return this.httpService.post<MatchParlayLocking[]>(url, data);
  }

  deleteItem(data: any) {
    const url = `${this.baseUrl}/delete-settlement-lock`;

    this.httpService.post(url, data).subscribe((res) => {
      this.dataUpdateSource.next(res);
    });
  }
}
