import { Component, OnInit } from '@angular/core';
import { Log } from './update-log-status.model';
import { ConfirmDialogComponent } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { HttpService } from '../shared/services/http.service';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';

@Component({
  selector: 'app-update-log-status',
  templateUrl: './update-log-status.component.html',
  styleUrls: ['./update-log-status.component.scss'],
})
export class UpdateLogStatusComponent implements OnInit {
  public logs!: Log[];

  constructor(
    private http: HttpService,
    public dialogService: ConfirmDialogService
  ) {}

  ngOnInit(): void {
    this.logs = [
      {
        dbServer: '',
        logID: 'ABc',
        statusID: 'St',
        startTime: new Date(),
        endTime: new Date(),
        startTimeValidation: new Date(),
      },
    ];
    // this.loadData();
  }

  loadData() {
    // this.http.getData<Log[]>('api/values').subscribe((data) => {
    //   this.logs = data;
    // });
    this.logs = [];
  }

  openUpdateConfirmDialog(log: Log) {
    const message = `Are your you want to <b>UPDATE</b> StatusId of LogID ${log.logID} to <b>Completed(2) for ${log.dbServer} ?`;

    const dialogRef = this.dialogService.open({ message });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      dialogResult && this.handleUpdateLog();
    });
  }

  handleUpdateLog() {
    //TO DO
    console.log('handle Update log');
  }

  openDeleteConfirmDialog(log: Log) {
    const message = `Are your you want to <b>DELETE</b> StatusId of LogID ${log.logID} to <b>Completed(2) for ${log.dbServer} ?`;

    const dialogRef = this.dialogService.open({ message });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      dialogResult && this.handleDeleteLog();
    });
  }

  handleDeleteLog() {
    //TO DO
    console.log('handle Delete log');
  }
}
