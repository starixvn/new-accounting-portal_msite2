import { Component, OnInit } from '@angular/core';
import { LogStatus } from './update-log-status.model';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { UpdateLogStatusService } from './update-log-status.service';
import { MatDialog } from '@angular/material/dialog';
import { UpdateLogStatusDetailComponent } from './update-log-status-detail.component';
import { groupBy } from 'lodash';
import { FileService } from '../shared/services/file.service';

@Component({
    selector: 'app-update-log-status',
    templateUrl: './update-log-status.component.html',
    styleUrls: ['./update-log-status.component.scss'],
})
export class UpdateLogStatusComponent implements OnInit {
    public logs!: LogStatus[];
    public nonSportLogs!: LogStatus[];
    public collapseSport: boolean = false;
    public collapseNonSport: boolean = false;

    constructor(
        private updateLogStatusService: UpdateLogStatusService,
        public dialogService: ConfirmDialogService,
        private dialog: MatDialog,
        private fileService: FileService
    ) { }

    ngOnInit(): void {
        this.loadData();
        this.updateLogStatusService.dataUpdate$.subscribe((res) => {
            this.loadData();
            this.dialog.closeAll();
        });
    }

    public get logsProcessed() {
        return Object.values(groupBy(this.logs || [], 'dbServer'));
    }

    public get nonSportLogsProcessed() {
        return Object.values(groupBy(this.nonSportLogs || [], 'dbServer'));
    }

    loadData() {
        this.updateLogStatusService.getAll().subscribe((res) => {
            this.logs = res.filter(r => r.queueType === 1);
            this.nonSportLogs = res.filter(r => r.queueType === 3)
        });
    }

    toggleSportBook() {
        this.collapseSport = !this.collapseSport;
    }
    toggleNonSport() {
        this.collapseNonSport = !this.collapseNonSport;
    }

    openDetailLogStatusDialog(log: LogStatus) {
        const config = {
            data: log,
            width: '600px',
            panelClass: 'detail-log-dialog',
        };
        this.dialog.open(UpdateLogStatusDetailComponent, config);
    }

    openUpdateConfirmDialog(log: LogStatus) {
        // this.updateLogStatusService.openUpdateConfirmDialog(log);
    }

    handleUpdateLogStatus(logs: LogStatus[]) { }

    exportFile(logs: LogStatus[]) {

        let data = '';

        logs.forEach(log => {
            data += `${log.logId}`;
            data += `\n${log.transInfoJson ?? ''}\n`;
        })
        this.fileService.saveTextAsFile(data, `transIds_of_${logs.map(v => v.logId).join('_')}`)
    }

}
