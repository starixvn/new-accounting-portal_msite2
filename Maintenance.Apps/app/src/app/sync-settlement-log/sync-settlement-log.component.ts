import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { DetailsSettlementLogComponent } from './details-settlement-log.component';
import { SettlementErrorLog } from './sync-settlement-log.model';
import { SettlementErrorLogService } from './sync-settlement-log.service';
import { groupBy } from "lodash";

@Component({
    selector: 'app-sync-settlement-log',
    templateUrl: './sync-settlement-log.component.html',
    styleUrls: ['./sync-settlement-log.component.scss']
})
export class SyncSettlementLogComponent implements OnInit {

    public logs!: SettlementErrorLog[];
    public matchId?: number;

    constructor(
        public dialogService: ConfirmDialogService,
        public settlementErrorLogService: SettlementErrorLogService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.loadData();
    }

    public get processedLogs() {
        const data = groupBy(this.logs, (item: SettlementErrorLog) => `${item.dbServer}+${item.siteDBGroupId}+${item.sessionId || item.messageId}`)
        const result = Object.values(data).map((v) => {
            const first = (v as any[])[0];
            first.tableNames = (v as any[]).map(v => v.tableName);
            return first;
        })

        return result as SettlementErrorLog[];
    }

    loadData() {
        this.settlementErrorLogService.getAll(this.matchId ?? 0).subscribe(res => {
            this.logs = res;
        })
    }

    submit() {
        this.loadData();
    }

    openConfirmDeleteSettlementLockDialog(item: SettlementErrorLog) {
        const message = `Session info
        <div>DB Server: ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>Action: Delete SiteDB_SettlementLock    </div>

        <div>Are you sure you want to <span class="red bold">DELETE</span> this session?    </div>
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementLock(item);
        });
    }
    confirmDeleteSettlementLock(item: SettlementErrorLog) {
        this.settlementErrorLogService.deleteInLogSettlementLock(item).subscribe(res => {
            this.loadData();
        })
    }


    openConfirmDeleteSettlementProcessingDialog(item: SettlementErrorLog, action: string) {
        const message = `Session info
        <div>DB Server: ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>Action: ${action === 'D' ? 'Delete' : 'Update'} SiteDB_SettlementProcessing    </div>

       ${action === 'D' ? '<div><i>Please ensure you check and delete data in SiteDB_SettlementProcessing</i></div>' : ''}
        <div>Are you sure you want to ${action === 'D' ? '<span class="red bold">DELETE</span>' : '<span class="blue bold">UPDATE</span>'} ${item.sessionId} logID belong to ${item.sessionId}?</div>
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementProcessing(item, action)
        });
    }
    confirmDeleteSettlementProcessing(item: SettlementErrorLog, action: string) {
        this.settlementErrorLogService.deleteInSettlementProcessing(item, action).subscribe(res => {
            this.loadData();
        })
    }

    openConfirmUpdateSSBDialog(item: SettlementErrorLog) {
        const message = `Session info
        <div>DB Server:  ${item.dbServer}    </div>
        <div>Session: ${item.sessionId}    </div>
        <div>MessageId: ${item.messageId}    </div>
        <div>Action: Update SSB_SettlementLogs    </div>

        <div>Are you sure you want to <span class="blue bold">UPDATE</span> Resolve=1?    </div>
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmUpdateSettlementLogs(item);
        });
    }
    confirmUpdateSettlementLogs(item: SettlementErrorLog) {
        this.settlementErrorLogService.updateInSSBSettlementLogs(item).subscribe(res => {
            this.loadData()
        })
    }

    openDetailsDialog(item: SettlementErrorLog) {
        if (!item.tableNames.includes('SiteDB_SettlementProcessing')) return;

        this.settlementErrorLogService.getDetails(item).subscribe(res => {
            console.log(res)
            const config = {
                data: { sessionId: item.sessionId, logs: res },
                width: '600px',
                panelClass: 'detail-log-dialog',
            };
            this.dialog.open(DetailsSettlementLogComponent, config);
        })




    }

}
