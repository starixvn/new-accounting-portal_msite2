import { Component, OnInit } from '@angular/core';
import { HttpService } from '../shared/services/http.service';

export interface MatchSettlementLock {
  matchId: number;
  sessionId: string;
  createdTime: string;
  homeTeam: string;
  awayTeam: string;
  totalSettlementProcessing: number;
  dBServer: string;
  errorMessage: string;
}

@Component({
  selector: 'app-delete-settlement-lock',
  templateUrl: './delete-settlement-lock.component.html',
  styleUrls: ['./delete-settlement-lock.component.scss'],
})
export class DeleteSettlementLockComponent implements OnInit {
  public matches: MatchSettlementLock[] | null = null;
  public matchId!: string;

  constructor(private httpService: HttpService) {}

  ngOnInit(): void {}

  loadData() {
    this.httpService
      .post<MatchSettlementLock[]>(
        'api/delete-temp-variable/get-settlement-lock',
        {
          matchId: this.matchId,
        }
      )
      .subscribe((data) => {
        this.matches = data ?? [];
      });
  }

  submit() {
    this.loadData();
  }
}
