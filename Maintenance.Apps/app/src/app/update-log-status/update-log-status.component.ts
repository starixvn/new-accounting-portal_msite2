import { Component, OnInit } from '@angular/core';
import { LogStatus } from './update-log-status.model';
import { ConfirmDialogComponent } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { HttpService } from '../shared/services/http.service';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { UpdateLogStatusService } from './update-log-status.service';
import { MatDialog } from '@angular/material/dialog';
import { UpdateLogStatusDetailComponent } from './update-log-status-detail.component';

@Component({
  selector: 'app-update-log-status',
  templateUrl: './update-log-status.component.html',
  styleUrls: ['./update-log-status.component.scss'],
})
export class UpdateLogStatusComponent implements OnInit {
  public logs!: LogStatus[];

  constructor(
    private updateLogStatusService: UpdateLogStatusService,
    public dialogService: ConfirmDialogService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.updateLogStatusService.dataUpdate$.subscribe((res) => {
      this.loadData();
      this.dialog.closeAll();
    });
  }

  loadData() {
    this.updateLogStatusService.getAll().subscribe((res) => {
      this.logs = res;
    });
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
    this.updateLogStatusService.openUpdateConfirmDialog(log);
  }

  openDeleteConfirmDialog(log: LogStatus) {
    this.updateLogStatusService.openDeleteConfirmDialog(log);
  }
}
