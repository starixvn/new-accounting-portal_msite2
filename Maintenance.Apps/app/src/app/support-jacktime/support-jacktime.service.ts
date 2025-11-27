import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable()
export class SupportJacktimeService {
      private readonly baseUrl = 'api/delete-temp-variable';
      private readonly httpClient = inject(HttpClient);

}