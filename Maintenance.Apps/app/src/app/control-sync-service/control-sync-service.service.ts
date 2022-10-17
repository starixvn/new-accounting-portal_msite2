import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';

interface InfoService {
  status: boolean;
}

@Injectable()
export class ControlSyncService {
  private baseUrl = 'api/control-sync-service';

  constructor(private httpService: HttpService) {}

  public updateStatus(status: Boolean) {
    const url = `${this.baseUrl}/UpdateStatus`;
    return this.httpService.post(url, { status });
  }

  public getInfoService() {
    const url = `${this.baseUrl}/GetInfoService`;

    return this.httpService.post<InfoService>(url);
  }
}
