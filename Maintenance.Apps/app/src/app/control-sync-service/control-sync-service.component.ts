import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './control-sync-service.component.html',
  styleUrls: ['./control-sync-service.component.scss'],
})
export class ControlSyncServiceComponent implements OnInit, AfterViewInit {
  public seconds = 0;
  public selectedSeconds = 20;
  private defaultTime: any;

  constructor(private dialogService: ConfirmDialogService) {}

  ngAfterViewInit(): void {
    this.defaultTime = this.handleInterval();
  }

  ngOnInit(): void {
    this.seconds = this.selectedSeconds;
  }

  onChanged(e: any) {
    clearInterval(this.defaultTime);
    this.loadData();

    this.selectedSeconds = e.target.value;
    this.seconds = this.selectedSeconds;

    this.handleInterval();
  }

  handleInterval() {
    return setInterval(() => {
      this.seconds = this.seconds - 1;
      if (this.seconds <= 0) {
        this.seconds = this.selectedSeconds;
        this.loadData();
        this.seconds = this.selectedSeconds;
      }
    }, 1000);
  }

  loadData() {
    console.log('Load Data');
  }

  openConfirmDialog(e: any) {
    const status = e.checked ? 'START' : 'STOP';
    const message = `Are you sure you want to ${status} Sync service for all SiteDB`;

    this.dialogService
      .open({ message })
      .afterClosed()
      .subscribe((res: Boolean) => {
        this.handleConfirm(res);
      });
  }

  handleConfirm(status: Boolean) {}
}
