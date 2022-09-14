import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/services/http.service';

@Component({
  selector: 'app-update-system-settings',
  templateUrl: './update-system-settings.component.html',
  styleUrls: ['./update-system-settings.component.scss'],
})
export class UpdateSystemSettingsComponent implements OnInit {
  public data: any[] | null = null;

  constructor(private httpService: HttpService) {}

  ngOnInit(): void {}

  loadData() {
    this.httpService
      .post('api/delete-temp-variable/get-system-setting')
      .subscribe((data) => {
        console.log('get-system-setting', data);
      });
  }

  onUpdate() {
    this.httpService
      .post('api/delete-temp-variable/update-system-setting')
      .subscribe((data) => {
        console.log('update-system-setting', data);
      });
  }
}
