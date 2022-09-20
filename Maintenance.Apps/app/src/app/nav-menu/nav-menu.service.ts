import { Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { Menu } from './nav-menu.model';

@Injectable()
export class NavMenuService {
  private baseUrl = 'api/values';

  constructor(private httpService: HttpService) {}

  getMenu() {
    const url = `${this.baseUrl}/get-menu`;

    return this.httpService.getData<Menu[]>(url);
  }
}
