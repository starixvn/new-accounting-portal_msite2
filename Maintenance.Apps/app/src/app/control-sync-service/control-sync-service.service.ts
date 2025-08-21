import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { LastLog } from './control-sync-service.model';

@Injectable()
export class ControlSyncService {
  private readonly baseUrl = 'api/delete-temp-variable';

  constructor(private readonly httpService: HttpService) { }

  public updateStatus(status: boolean, cacheKey: string) {
    const url = `${this.baseUrl}/update-status`;
    return this.httpService.post(url, { status, cacheKey });
  }

  public getInfoService() {
    const url = `${this.baseUrl}/get-info-service`;

    return this.httpService.post<LastLog[]>(url);
  }
}
