import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';

@Component({
    selector: 'app-sync-settlement-log-tool',
    templateUrl: './sync-settlement-log-tool.component.html',
    styleUrls: ['./sync-settlement-log-tool.component.scss']
})
export class SyncSettlementLogToolComponent implements OnInit {

    constructor(
        public dialogService: ConfirmDialogService
    ) { }

    ngOnInit(): void {
    }

    openConfirmDeleteSettlementLockDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Delete SiteDB_SettlementLock

        Are you sure you want to DELETE this row?
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementLock();
        });
    }
    confirmDeleteSettlementLock() { }


    openConfirmUpdateSettlementLockDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Delete SiteDB_SettlementLock

        Are you sure you want to DELETE this row?
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmUpdateSettlementLock();
        });
    }
    confirmUpdateSettlementLock() { }

    openConfirmDeleteSettlementProcessingDialog() {
        const message = `Session info
        DB Server:
        Session:
        Action: Delete SiteDB_SettlementProcessing

        <i>Please ensure you check and delete data in SiteDB_SettlementProcessing</i>
        Are you sure you want to UPDATE <> rows belong to <>? 
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmDeleteSettlementProcessing()
        });
    }
    confirmDeleteSettlementProcessing() { }

    openConfirmUpdateSettlementLogsDialog() {
        const message = `Session info
        DB Server:
        Session:
        MessageId: 
        Action: Update SSB_SettlementLogs

        Are you sure you want to UPDATE this row?
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmUpdateSettlementLogs();
        });
    }
    confirmUpdateSettlementLogs() { }

    openConfirmUpdateRepUpdateTimeDialog() {
        const message = `Session info
        DB Server:
        Session:
        MessageId:
        Action: Update ReUpdateTime

        Are you sure you want to UPDATE RepUpdateTime TransId belong to <>?
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmUpdateRepUpdateTime();
        });
    }
    confirmUpdateRepUpdateTime() { }

    openConfirmActiveParallelQueueDialog() {
        const message = `Session info
        DB Server:
        Action: Reactive Parallel Queue

        Are you sure you want to REACTIVE PARALLEL QUEUE of <>?
        `;
        const dialogRef = this.dialogService.open({ message });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.confirmActiveParallelQueue();
        });
    }
    confirmActiveParallelQueue() { }

}
