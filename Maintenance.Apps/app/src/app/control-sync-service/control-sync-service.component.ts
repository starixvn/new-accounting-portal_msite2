import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { ControlSyncService } from './control-sync-service.service';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './control-sync-service.component.html',
  styleUrls: ['./control-sync-service.component.scss'],
})
export class ControlSyncServiceComponent implements OnInit, AfterViewInit {
  public countDownSeconds = 0;
  public selectedSeconds = 60;
  public secondsOptions = [30, 60, 90, 120];
  public currentStatus = false;
  private defaultTimer: any;
  readonly INTERVAL_TIME = 1000;

  constructor(
    private dialogService: ConfirmDialogService,
    private controlSyncService: ControlSyncService
  ) {}

  ngAfterViewInit(): void {
    this.defaultTimer = this.handleInterval();
  }

  ngOnInit(): void {
    this.countDownSeconds = this.selectedSeconds;
    this.loadData();
  }

  onChangedTimerDropdown(e: any) {
    clearInterval(this.defaultTimer);
    this.loadData();

    this.selectedSeconds = e.target.value;
    this.countDownSeconds = this.selectedSeconds;

    this.handleInterval();
  }

  handleInterval() {
    return setInterval(() => {
      this.countDownSeconds = this.countDownSeconds - 1;
      if (this.countDownSeconds <= 0) {
        this.countDownSeconds = this.selectedSeconds;
        this.loadData();
      }
    }, this.INTERVAL_TIME);
  }

  loadData() {
    this.controlSyncService.getInfoService().subscribe((res) => {
      this.currentStatus = res.status;
    });
  }

  openConfirmDialog(e: any) {
    this.currentStatus = e.checked;
    const contentStatus = e.checked ? 'START' : 'STOP';
    const message = `Are you sure you want to <span class="${
      e.checked ? 'blue' : 'red'
    }">${contentStatus}</span> Sync service for all SiteDB`;

    this.dialogService
      .open({ message })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        confirm && this.handleConfirm();
      });
  }

  handleConfirm() {
    this.controlSyncService
      .updateStatus(this.currentStatus)
      .subscribe((res) => {
        this.loadData();
      });
  }
}
