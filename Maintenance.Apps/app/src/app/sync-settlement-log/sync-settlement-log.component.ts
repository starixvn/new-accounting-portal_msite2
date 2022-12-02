import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { DetailsSettlementLogComponent } from './details-settlement-log.component';
import { SettlementErrorLog } from './sync-settlement-log.model';
import { SettlementErrorLogService } from './sync-settlement-log.service';

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

    loadData() {
        this.settlementErrorLogService.getAll(this.matchId ?? 0).subscribe(res => {
            this.logs = res;
        })
    }

    openConfirmDeleteSettlementLockDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Delete SiteDB_SettlementLock

        Are you sure you want to <span class="red">DELETE</span> this session?
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementLock();
        });
    }
    confirmDeleteSettlementLock() { }

    openConfirmDeleteSettlementProcessingDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Delete SiteDB_SettlementProcessing

        <i>Please ensure you check and delete data in SiteDB_SettlementProcessing</i>
        Are you sure you want to <span class="red">DELETE</span> logID belong to <>? 
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementProcessing()
        });
    }
    confirmDeleteSettlementProcessing() { }


    openConfirmUpdateSettlementProcessingDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Update SiteDB_SettlementProcessing

        <i>Please ensure you check and delete data in SiteDB_SettlementProcessing</i>
        Are you sure you want to <span class="blue">UPDATE</span> LogIds belong to ? 
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementProcessing()
        });
    }
    confirmUpdateSettlementProcessing() { }

    openConfirmUpdateSettlementLogsDialog() {
        const message = `Session info
        DB Server:
        Session:
        MessageId: 
        Action: Update SSB_SettlementLogs

        Are you sure you want to <span class="blue">UPDATE</span> Resolve=1?
        `;
        const dialogRef = this.dialogService.openConfirm({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmUpdateSettlementLogs();
        });
    }
    confirmUpdateSettlementLogs() { }

    openDetailsDialog() {
        const config = {
            data: null,
            width: '600px',
            panelClass: 'detail-log-dialog',
        };
        this.dialog.open(DetailsSettlementLogComponent, config);
    }

}
