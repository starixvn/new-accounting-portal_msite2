import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { SystemSettings } from './update-system-settings.model';

@Injectable()
export class UpdateSystemSettingService {
  private baseUrl = 'api/delete-temp-variable';

  constructor(private httpService: HttpService) {}

  getAll() {
    return this.httpService.post<SystemSettings[]>(
      `${this.baseUrl}/get-system-setting`
    );
  }

  update(dbServer: string) {
    return this.httpService.post(`${this.baseUrl}/update-system-setting`, {
      dbServer,
    });
  }
}
