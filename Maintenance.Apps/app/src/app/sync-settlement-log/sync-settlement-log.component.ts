import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { DetailsSettlementLogComponent } from './details-settlement-log.component';
import { SettlementErrorLog } from './sync-settlement-log.model';
import { SettlementErrorLogService } from './sync-settlement-log.service';
import { groupBy, orderBy, compact } from 'lodash';

@Component({
  selector: 'app-sync-settlement-log',
  templateUrl: './sync-settlement-log.component.html',
  styleUrls: ['./sync-settlement-log.component.scss'],
})
export class SyncSettlementLogComponent implements OnInit {
  public logs: SettlementErrorLog[] = [];
  public matchId = '';
  public typeSettle = 'single';
  public settlementLockTitle = 'SiteDB_SettlementLock';
  public settlementProcessingTitle = 'SiteDB_SettlementProcessing';

  constructor(
    public readonly dialogService: ConfirmDialogService,
    public readonly settlementErrorLogService: SettlementErrorLogService,
    private readonly dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private processLogs(logs: SettlementErrorLog[]) {
    const data = groupBy(
      logs,
      (item: SettlementErrorLog) =>
        `${item.dbServer ?? ''}+${item.siteDBGroupId}+${item.sessionId || item.messageId
        }`
    );
    const result = Object.values(data).map((v) => {
      const first = v.find((e) => !!e) as SettlementErrorLog;

      first.createdTime = compact(v.map((x) => x.createdTime))[0];
      first.eventName = compact(v.map((x) => x.eventName))[0];
      first.errorMessages = compact(v.map((x) => x.errorMessages))[0];
      first.messageId = compact(v.map((x) => x.messageId))[0];
      first.tableNames = (v as SettlementErrorLog[]).map((x) => x.tempTableName);

      return first;
    });

    return orderBy(
      result,
      ['matchId', 'sessionId', 'createdTime'],
      ['desc']
    ) as SettlementErrorLog[];
  }

  loadData() {
    this.settlementErrorLogService
      .getAll(+this.matchId, this.typeSettle)
      .subscribe((res) => {
        this.settlementLockTitle =
          this.typeSettle === 'parlay'
            ? 'SiteDB_ParlaySettlementLock'
            : 'SiteDB_SettlementLock';
        this.settlementProcessingTitle =
          this.typeSettle === 'parlay'
            ? 'SiteDB_ParlaySettlementProcessing'
            : 'SiteDB_SettlementProcessing';
        this.logs = this.processLogs(res);
      });
  }

  submit() {
    this.loadData();
  }

  openConfirmDeleteSettlementLockDialog(item: SettlementErrorLog) {
    const message = `Session info
        <div>DB Server: ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>Action: Delete ${this.settlementLockTitle}    </div>
        <div><i>Please ensure you check and delete data in ${this.settlementProcessingTitle}</i></div>
        <div>Are you sure you want to <span class="red bold">DELETE</span> this session?    </div>
        `;
    const dialogRef = this.dialogService.openConfirm({ message });

    dialogRef.afterClosed().subscribe((res) => {
      res && this.confirmDeleteSettlementLock(item);
    });
  }
  confirmDeleteSettlementLock(item: SettlementErrorLog) {
    this.settlementErrorLogService
      .deleteInLogSettlementLock(item, this.typeSettle)
      .subscribe((res) => {
        this.loadData();
      });
  }

  existsSettlementProcessing(item: SettlementErrorLog): boolean {
    return item.tableNames.some((x) =>
      x.includes('SiteDB_SettlementProcessing')
    );
  }

  openConfirmDeleteSettlementProcessingDialog(
    item: SettlementErrorLog,
    action: string
  ) {
    const message = `Session info
        <div>DB Server: ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>Action: ${action === 'D' ? 'Delete' : 'Update'} ${this.settlementProcessingTitle
      }    </div>
        <div>Are you sure to ${action === 'D'
        ? '<span class="red bold">DELETE</span>'
        : '<span class="blue bold">UPDATE</span>'
      } Failed/In-progress logIDs belong to ${item.sessionId} ${action === 'U' ? 'to Waiting' : ''
      }?</div>
        `;
    const dialogRef = this.dialogService.openConfirm({ message });

    dialogRef.afterClosed().subscribe((res) => {
      res && this.confirmDeleteSettlementProcessing(item, action);
    });
  }
  confirmDeleteSettlementProcessing(item: SettlementErrorLog, action: string) {
    this.settlementErrorLogService
      .deleteInSettlementProcessing(item, action, this.typeSettle)
      .subscribe((res) => {
        this.loadData();
      });
  }

  openConfirmUpdateSSBDialog(item: SettlementErrorLog) {
    const message = `Session info
        <div>DB Server:  ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>MessageId: ${item.messageId ?? ''}    </div>
        <div>Action: Update SSB_SettlementLogs    </div>

        <div>Are you sure you want to <span class="blue bold">UPDATE</span> Resolve=1?    </div>
        `;
    const dialogRef = this.dialogService.openConfirm({ message });

    dialogRef.afterClosed().subscribe((res) => {
      res && this.confirmUpdateSettlementLogs(item);
    });
  }
  confirmUpdateSettlementLogs(item: SettlementErrorLog) {
    this.settlementErrorLogService
      .updateInSSBSettlementLogs(item)
      .subscribe((res) => {
        this.loadData();
      });
  }

  openDetailsDialog(item: SettlementErrorLog) {
    if (!this.existsSettlementProcessing(item)) {
      return;
    }

    this.settlementErrorLogService
      .getDetails(item, this.typeSettle)
      .subscribe((res) => {
        const newLogs = res.map((x) => ({
          ...x,
          tableName: item.tableName,
          functionName: item.functionName,
          settlementAction: item.settlementAction,
          ticketType: item.ticketType,
          ticketTypeExplicit: item.ticketTypeExplicit,
        }));

        const config = {
          data: { sessionId: item.sessionId, logs: newLogs },
          width: '700px',
          panelClass: 'detail-log-dialog',
        };
        this.dialog.open(DetailsSettlementLogComponent, config);
      });
  }
}
