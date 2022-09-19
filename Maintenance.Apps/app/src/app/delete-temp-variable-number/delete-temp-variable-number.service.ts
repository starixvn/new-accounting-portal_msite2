import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpService } from '../shared/services/http.service';
import { Match } from './match.model';

@Injectable()
export class DeleteTempVariableNumberService {
  private baseUrl = 'api/delete-temp-variable';

  private dataUpdateSource = new Subject();
  dataUpdate$ = this.dataUpdateSource.asObservable();

  constructor(private httpService: HttpService) {}

  getAll(matchId: string) {
    const url = `${this.baseUrl}/get-temp-variable`;
    const body = {
      matchId,
    };
    return this.httpService.post<Match[]>(url, body);
  }

  getDetail(data: any) {
    const url = `${this.baseUrl}/get-detail-temp-variable`;
    const item = {
      ...data,
      ActionName: data.action,
    };

    return this.httpService.post<Match>(url, item);
  }

  deleteItem(data: any) {
    const url = `${this.baseUrl}/delete-temp-variable`;
    const body = { ...data };

    this.httpService.post(url, body).subscribe((res: any) => {
      this.dataUpdateSource.next(res);
    });
  }
}
