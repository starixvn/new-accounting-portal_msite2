import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { Observable } from 'rxjs';
import { SiteDB } from './site-db.model';

@Injectable()
export class UpdateFlagSitedbService {
  private readonly baseUrl = 'api/sitedb';

  constructor(private readonly httpService: HttpService) { }

  public getSiteDBs(): Observable<SiteDB[]> {
    const url = `${this.baseUrl}/get-all-sitedb`;
    return this.httpService.getData<SiteDB[]>(url);
  }

  public updateSiteDBStatus(siteDB: SiteDB): Observable<SiteDB> {
    const url = `${this.baseUrl}/update-status`;
    return this.httpService.post<SiteDB>(url, siteDB);
  }

  public addSiteDB(siteDB: SiteDB): Observable<SiteDB> {
    const url = `${this.baseUrl}/add-sitedb`;
    return this.httpService.post<SiteDB>(url, siteDB);
  }

  public removeSiteDB(siteDB: SiteDB): Observable<boolean> {
    const url = `${this.baseUrl}/remove-sitedb`;
    return this.httpService.post<boolean>(url, siteDB);
  }
}
