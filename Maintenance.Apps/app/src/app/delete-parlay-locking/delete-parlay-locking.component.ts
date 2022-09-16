import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { MatchParlayLockingService } from './delete-parlay-locking.service';
import {
  ParlayLockingRequest,
  MatchParlayLocking,
} from './match-parlay-locking.model';

@Component({
  selector: 'app-delete-parlay-locking',
  templateUrl: './delete-parlay-locking.component.html',
  styleUrls: ['./delete-parlay-locking.component.scss'],
})
export class DeleteParlayLockingComponent implements OnInit {
  public filter: ParlayLockingRequest = { timeRange: 1 };
  public timeRanges: number[] = [1, 12, 24];
  public matches: MatchParlayLocking[] | null = null;

  constructor(
    private matchService: MatchParlayLockingService,
    private dialogService: ConfirmDialogService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  submit() {
    this.loadData();
  }

  loadData() {
    this.matchService.getAll(this.filter).subscribe((data) => {
      this.matches = data;
    });
  }

  openDeleteConfirmDialog(item: MatchParlayLocking) {
    const message = `
      <div>DB Server: ${item.dbServer}</div>
      <div>MatchId:  ${item.matchId} </div>
      <div>Total Affected Refno(s): ${item.totalAffectedRefno} </div>
      <div>Session: ${item.session}</div>
      <div>Are you sure you want to DELETE this row?
    `;

    this.dialogService
      .open({ message })
      .afterClosed()
      .subscribe((res: Boolean) => {
        res && this.handleDeleteItem(item);
      });
  }

  handleDeleteItem(item: MatchParlayLocking) {
    this.matchService.deleteItem(item).subscribe((res) => {
      this.loadData();
    });
  }
}
