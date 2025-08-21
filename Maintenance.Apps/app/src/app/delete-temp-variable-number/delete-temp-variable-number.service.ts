import { Injectable } from '@angular/core';
import * as _ from 'lodash';
import { Subject } from 'rxjs';
import { HttpService } from '../shared/services/http.service';
import { Match } from './match.model';

@Injectable()
export class DeleteTempVariableNumberService {
  private readonly baseUrl = 'api/delete-temp-variable';

  private readonly dataUpdateSource = new Subject();
  dataUpdate$ = this.dataUpdateSource.asObservable();

  constructor(private readonly httpService: HttpService) { }

  getAll(matchId?: number) {
    matchId = _.isNumber(matchId) ? matchId : 0;
    const url = `${this.baseUrl}/get-temp-variable`;
    const body = {
      matchId,
    };
    return this.httpService.post<Match[]>(url, body);
  }

  getDetail(data: Match) {
    const url = `${this.baseUrl}/get-detail-temp-variable`;
    const item = {
      ...data,
      ActionName: data.action,
    };

    return this.httpService.post<Match>(url, item);
  }

  deleteItem(data: Match) {
    const url = `${this.baseUrl}/delete-temp-variable`;
    const body = { ...data };

    this.httpService.post(url, body).subscribe((res) => {
      this.dataUpdateSource.next(res);
    });
  }
}
