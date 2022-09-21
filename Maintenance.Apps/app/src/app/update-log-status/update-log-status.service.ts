import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
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
  ) {}

  getAll() {
    const url = `${this.baseUrl}/get-log-status`;
    return this.httpService.post<LogStatus[]>(url);
  }

  updateItem(item: LogStatus) {
    const url = `${this.baseUrl}/update-log-status`;
    const body: UpdateLogStatusRequest = {
      ...item,
      action: 'Update',
    };

    this.httpService.post(url, body).subscribe((res) => {
      this.dataUpdateSource.next(res);
    });
  }

  deleteItem(item: LogStatus) {
    const url = `${this.baseUrl}/update-log-status`;
    const body: UpdateLogStatusRequest = {
      ...item,
      action: 'Delete',
    };

    this.httpService.post(url, body).subscribe((res) => {
      this.dataUpdateSource.next(res);
    });
  }

  openUpdateConfirmDialog(log: LogStatus) {
    const message = `Are your you want to <span class="blue">UPDATE</span> StatusID of LogID ${log.logId} to <b>Completed(2) for ${log.serverName}?`;

    const dialogRef = this.dialogService.open({ message });

    dialogRef.afterClosed().subscribe((res) => {
      res && this.updateItem(log);
    });
  }

  openDeleteConfirmDialog(log: LogStatus) {
    const message = `Are your you want to <span class="red">DELETE</span> log of LogID ${log.logId} for ${log.serverName}?`;

    const dialogRef = this.dialogService.open({ message });

    dialogRef.afterClosed().subscribe((res) => {
      res && this.deleteItem(log);
    });
  }
}
