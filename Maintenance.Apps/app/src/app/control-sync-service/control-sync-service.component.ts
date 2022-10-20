import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { ControlSyncService } from './control-sync-service.service';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './control-sync-service.component.html',
  styleUrls: ['./control-sync-service.component.scss'],
})
export class ControlSyncServiceComponent implements OnInit {
  public currentStatus = false;

  constructor(
    private dialogService: ConfirmDialogService,
    private controlSyncService: ControlSyncService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.controlSyncService.getInfoService().subscribe((res) => {
      this.currentStatus = res.status;
    });
  }

  openConfirmDialog(e: any) {
    const contentStatus = e.checked ? 'START' : 'STOP';
    const message = `Are you sure you want to <span class="${
      e.checked ? 'blue' : 'red'
    }"><b>${contentStatus}</b></span> Sync service for all SiteDB`;

    const confirmText = e.checked ? 'Start' : 'Stop';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';

    this.dialogService
      .open({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.currentStatus = e.checked;
          this.handleConfirm();
        } else {
          e.source.checked = this.currentStatus;
        }
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
