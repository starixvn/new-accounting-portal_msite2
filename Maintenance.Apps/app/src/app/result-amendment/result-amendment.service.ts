import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { AmendmentResultUpdateRequest, ResultAmendment } from './type';

@Injectable()
export class ResultAmendmentService {
  private readonly baseUrl = 'api/delete-temp-variable';

  constructor(private readonly httpService: HttpService) {}

  public updateStatus(request: AmendmentResultUpdateRequest) {
    const url = `${this.baseUrl}/update-amendment-result`;
    return this.httpService.post(url, request);
  }

  public getAmenentResults(request: {
    matchId: number | null;
    statusId: number;
  }) {
    const url = `${this.baseUrl}/get-amendment-results`;

    return this.httpService.post<ResultAmendment[]>(url, request);
  }
}
