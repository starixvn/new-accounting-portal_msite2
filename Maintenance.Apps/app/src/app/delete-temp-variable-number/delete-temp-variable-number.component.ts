import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/services/http.service';
import { Match } from './match.model';

@Component({
  selector: 'app-delete-temp-variable-number',
  templateUrl: './delete-temp-variable-number.component.html',
  styleUrls: ['./delete-temp-variable-number.component.scss'],
})
export class DeleteTempVariableNumberComponent implements OnInit {
  constructor(private httpService: HttpService) {}

  public matches: Match[] | null = null;
  public matchId!: string;

  ngOnInit(): void {}

  loadData() {
    const params = new HttpParams();
    params.set('matchId', this.matchId);

    this.httpService
      .post<Match[]>('api/delete-temp-variable/get-temp-variable', null, params)
      .subscribe((data) => {
        this.matches = data;
      });
  }

  submit() {
    this.loadData();
  }
}
