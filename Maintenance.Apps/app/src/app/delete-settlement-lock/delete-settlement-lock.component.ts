import { Component, OnInit } from '@angular/core';
import { MatchSettlementLock } from './match-settlement-lock.model';
import { MatchSettlementLockService } from './match-settlement-lock.service';

@Component({
  selector: 'app-delete-settlement-lock',
  templateUrl: './delete-settlement-lock.component.html',
  styleUrls: ['./delete-settlement-lock.component.scss'],
})
export class DeleteSettlementLockComponent implements OnInit {
  public matches: MatchSettlementLock[] | null = null;
  public matchId!: string;

  constructor(private matchService: MatchSettlementLockService) {}

  ngOnInit(): void {}

  submit() {
    this.loadData();
  }

  loadData() {
    // this.matchService.getAll(this.matchId).subscribe((data) => {
    //   this.matches = data;
    // });
    this.matches = [
      {
        dBServer: 'DBTXN',
        matchId: 12223,
        sessionId: '34343',
        errorMessage: '',
        createdTime: '',
        homeTeam: 'ab',
        awayTeam: 'ABC',
        totalSettlementProcessing: 0,
      },
    ];
  }

  deleteItem(item: MatchSettlementLock) {
    // this.matchService.deleteItem(null)
  }

  updateItem(item: MatchSettlementLock) {
    // this.matchService.updateItem(item)
  }
}
