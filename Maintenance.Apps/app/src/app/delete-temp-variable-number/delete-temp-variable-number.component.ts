import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/services/http.service';

@Component({
  selector: 'app-delete-temp-variable-number',
  templateUrl: './delete-temp-variable-number.component.html',
  styleUrls: ['./delete-temp-variable-number.component.scss'],
})
export class DeleteTempVariableNumberComponent implements OnInit {
  constructor(private httpService: HttpService) {}

  public data: any[] = [];
  public matchId!: string;

  ngOnInit(): void {}

  loadData() {
    this.httpService.getData('api/values').subscribe((data) => {
      console.log(data);
    });
  }

  submit() {
    console.log(this.matchId);
  }
}
