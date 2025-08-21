import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { FlagBroadcast } from './update-flag-broadcast.model';

@Injectable()
export class UpdateFlagBroadcastService {
  private readonly baseUrl = 'api/delete-temp-variable';

  constructor(private readonly httpService: HttpService) { }

  public updateFlag(status: boolean, productType: number[]) {
    const url = `${this.baseUrl}/update-flag-broadcast-status`;
    return this.httpService.post(url, { status, productType });
  }

  public getInfo() {
    const url = `${this.baseUrl}/get-flag-broadcast-status`;

    return this.httpService.post<FlagBroadcast>(url);
  }

  public updateFlagUm(status: boolean) {
    const url = `${this.baseUrl}/update-flag-broadcast-um-status`;
    return this.httpService.post<boolean>(url, { status });
  }
}
