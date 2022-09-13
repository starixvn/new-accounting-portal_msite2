import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { HttpService } from 'src/services/http.service';
import { Log } from './log.model';

@Component({
  selector: 'app-update-log-status',
  templateUrl: './update-log-status.component.html',
  styleUrls: ['./update-log-status.component.scss'],
})
export class UpdateLogStatusComponent implements OnInit {
  public logs!: Log[];

  constructor(private http: HttpService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.logs = [];
    this.loadData();
  }

  loadData() {
    this.http.getData<Log[]>('api/values').subscribe((data) => {
      console.log(data);
      this.logs = data;
    });
  }

  openDialog() {
    // const dialogRef = this.dialog.open();
    // dialogRef.afterClosed().subscribe((result) => {
    //   console.log(`Dialog result: ${result}`);
    // });
  }
}
