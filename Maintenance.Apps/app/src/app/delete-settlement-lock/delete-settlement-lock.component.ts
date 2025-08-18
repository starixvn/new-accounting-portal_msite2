import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { SnackbarService } from '../shared/services/snackbar.service';
import {
  DeleteSettlementLog,
  MatchSettlementLock,
} from './match-settlement-lock.model';
import { MatchSettlementLockService } from './match-settlement-lock.service';

@Component({
  selector: 'app-delete-settlement-lock',
  templateUrl: './delete-settlement-lock.component.html',
  styleUrls: ['./delete-settlement-lock.component.scss'],
})
export class DeleteSettlementLockComponent implements OnInit {
  public matches: MatchSettlementLock[] = [];
  public matchId?: number;
  public typeSettle = 'single';
  public settlementLockTitle = 'SettlementLock';
  public settlementProcessingTitle = 'SettlementProcessing';

  constructor(
    private readonly matchService: MatchSettlementLockService,
    private readonly dialogService: ConfirmDialogService,
    private readonly snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  submit() {
    this.loadData();
  }

  loadData() {
    this.matchService
      .getAll(this.matchId, this.typeSettle)
      .subscribe((data) => {
        this.matches = data;
        this.settlementLockTitle =
          this.typeSettle === 'parlay'
            ? 'ParlaySettlementLock'
            : 'SettlementLock';
        this.settlementProcessingTitle =
          this.typeSettle === 'parlay'
            ? 'ParlaySettlementProcessing'
            : 'SettlementProcessing';
      });
  }

  openDeleteConfirmDialog(item: MatchSettlementLock, isSettleLock: boolean) {
    let message = '';
    if (isSettleLock) {
      message = `
        <div><b>MatchId:</b> ${item.matchId}</div>
        <div><b>Session:</b> ${item.sessionId}</div>
        <div><b>Action:</b> Delete ${this.settlementLockTitle} </div>
        <div>Are you sure you want to <span class="red">DELETE</span> this row?</div>`;
    } else {
      message =
        item.dbServer === 'DBTrans'
          ? 'Please check and delete locked match from function Delete Temp Variable Number before recalculate.\n'
          : '';
      message = `
      <div> ${message}</div>
        <div><b>MatchId:</b>
        ${item.matchId} </div>
        <div>Delete  ${item.totalSettlementProcessing} rows belong to sessionid ${item.sessionId} </div>
        <div><b>Action:</b> Delete ${this.settlementProcessingTitle} </div>
        <div>Are you sure you want to <span class="red">DELETE</span> this row?</div>`;
    }
    const confirmStyle = 'btn-confirm-delete';

    this.dialogService
      .openConfirm({ message, confirmStyle })
      .afterClosed()
      .subscribe((res) => {
        res &&
          this.deleteSettlementProcessing({
            dbServer: item.dbServer,
            sessionId: item.sessionId,
            isSettleLock,
          });
      });
  }

  deleteSettlementProcessing(item: DeleteSettlementLog) {
    this.matchService.deleteItem(item, this.typeSettle).subscribe((res) => {
      this.snackbarService.openSuccessSnackBar();
      this.loadData();
    });
  }

  openUpdateConfirmDialog(item: MatchSettlementLock) {
    const message = `
    <div>Please ensure you check and delete data in table ${this.settlementProcessingTitle} before updating:</div>
    <div><b>MatchId:</b> ${item.matchId}</div>
    <div><b>Session:</b> ${item.sessionId}</div>
    <div>Are you sure you want to UPDATE Resolved?</div>`;

    this.dialogService
      .openConfirm({ message })
      .afterClosed()
      .subscribe((res) => {
        res && this.updateSettlementLogs(item);
      });
  }

  updateSettlementLogs(item: MatchSettlementLock) {
    this.matchService.updateItem(item, this.typeSettle).subscribe(() => {
      this.snackbarService.openSuccessSnackBar();
      this.loadData();
    });
  }
}
