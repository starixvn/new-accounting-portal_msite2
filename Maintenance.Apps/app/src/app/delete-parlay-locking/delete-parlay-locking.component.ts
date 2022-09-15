import { Component, OnInit } from '@angular/core';
import { MatchParlayLockingService } from './delete-parlay-locking.service';
import { Filter, MatchParlayLocking } from './match-parlay-locking.model';

@Component({
  selector: 'app-delete-parlay-locking',
  templateUrl: './delete-parlay-locking.component.html',
  styleUrls: ['./delete-parlay-locking.component.scss'],
})
export class DeleteParlayLockingComponent implements OnInit {
  public filter: Filter = { timeRange: 1 };
  public timeRanges: number[] = [1, 12, 24];
  public matches: MatchParlayLocking[] | null = null;

  constructor(private matchService: MatchParlayLockingService) {}

  ngOnInit(): void {
    this.matchService.dataUpdate$.subscribe((res) => {
      this.loadData();
    });
  }

  submit() {
    this.loadData();
  }

  loadData() {
    this.matchService.getAll(this.filter).subscribe((data) => {
      //   this.matches = data;
      this.matches = [
        {
          matchId: 1,
          totalAffectedRefno: 1,
          session: 'ffefe',
          sPName: 'fefe',
          createdDate: 'fafa',
          dbServer: 'AB',
          serverName: 'ABSE',
        },
      ];
    });
  }

  onDelete(item: any) {
    this.matchService.deleteItem(item);
  }
}
