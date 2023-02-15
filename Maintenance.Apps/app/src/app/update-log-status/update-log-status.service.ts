import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { HttpService } from '../shared/services/http.service';
import { LogStatus, UpdateLogStatusRequest } from './update-log-status.model';

@Injectable()
export class UpdateLogStatusService {
    private baseUrl = 'api/delete-temp-variable';

    private dataUpdateSource = new Subject();
    public dataUpdate$ = this.dataUpdateSource.asObservable();

    constructor(
        private httpService: HttpService,
        public dialogService: ConfirmDialogService
    ) { }

    getAll() {
        const url = `${this.baseUrl}/get-log-status`;
        return this.httpService.post<LogStatus[]>(url);
    }

    checkLogIds(logs: LogStatus[]): Observable<boolean> {

        const url = `${this.baseUrl}/check-logIds`;
        const body = {
            logIds: logs.map(x => x.logId).join(","),
            dbServer: logs[0].dbServer
        };
        return this.httpService.post(url, body)
    }

    updateItem(logs: LogStatus[]) {
        const url = `${this.baseUrl}/update-log-status`;
        const body = {
            logIds: logs.map(v => v.logId).join(','),
            dbServer: logs[0].dbServer
        };

        this.httpService.post(url, body).subscribe((res) => {
            this.dataUpdateSource.next(res);
        });
    }


    openUpdateConfirmDialog(logs: LogStatus[]) {
        const logIds = logs.map(v => v.logId).join(', ');

        const message = `Are you sure you want to <span class="blue bold">UPDATE</span> StatusID of LogID <b>${logIds}</b> to <span class="blue bold">Completed</span> for <b>${logs[0].serverName}</b>?`;
        const confirmStyle = 'btn-confirm-update';

        const dialogRef = this.dialogService.openConfirm({ message, confirmStyle });

        dialogRef.afterClosed().subscribe((res) => {
            res && this.updateItem(logs);
        });
    }

    openWarningDialog() {
        const message = `<div>LogIds are not existed in CentralDB.</div>
        <div>Please check Sync Service again!</div>
        `
        this.dialogService.openWarning({ message })
    }
}
