import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';

@Injectable()
export class UpdateFlagRebateRetentionService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(private httpService: HttpService) {}

  public updateFlag(mode: number) {
    const url = `${this.baseUrl}/update-flag-rebate-retention`;
    return this.httpService.post<number>(url, { mode });
  }

  public getInfo() {
    const url = `${this.baseUrl}/get-flag-rebate-retention`;

    return this.httpService.post<number>(url);
  }
}
