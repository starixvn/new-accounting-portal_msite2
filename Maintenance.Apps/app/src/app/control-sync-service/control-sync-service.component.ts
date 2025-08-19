import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { CacheKey } from './constants';
import { LastLog } from './control-sync-service.model';
import { ControlSyncService } from './control-sync-service.service';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './control-sync-service.component.html',
  styleUrls: ['./control-sync-service.component.scss'],
})
export class ControlSyncServiceComponent implements OnInit {
  public sportbookStatus = false;
  public nonSportStatus = false;
  public sportBookLogs: LastLog[] = [];
  public nonSportLogs: LastLog[] = [];
  public collapseSportBook = false;
  public collapseNonSport = false;

  constructor(
    private dialogService: ConfirmDialogService,
    private controlSyncService: ControlSyncService
  ) {}

  ngOnInit(): void {
    this.loadInfoService();
  }

  toggleSportBook() {
    this.collapseSportBook = !this.collapseSportBook;
  }

  toggleNonSport() {
    this.collapseNonSport = !this.collapseNonSport;
  }

  loadInfoService() {
    this.controlSyncService.getInfoService().subscribe((res) => {
      const sportBookLogs = res.filter((v) => v.queueType === 1);
      const nonSportLogs = res.filter((v) => v.queueType === 3);

      this.sportBookLogs = sportBookLogs;
      this.sportbookStatus = sportBookLogs[0].currentStatus;

      this.nonSportLogs = nonSportLogs;
      this.nonSportStatus = nonSportLogs[0].currentStatus;
    });
  }

  openConfirmDialog(e) {
    const message = this.createDialogMessage(
      e.checked,
      CacheKey.SportsCacheKey
    );
    const confirmText = e.checked ? 'Start' : 'Stop';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';

    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.sportbookStatus = e.checked;
          this.handleConfirm(CacheKey.SportsCacheKey, this.sportbookStatus);
        } else {
          e.source.checked = this.sportbookStatus;
        }
      });
  }

  openConfirmNonSportDialog(e) {
    const message = this.createDialogMessage(
      e.checked,
      CacheKey.NonSportsCacheKey
    );
    const confirmText = e.checked ? 'Start' : 'Stop';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';

    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.nonSportStatus = e.checked;
          this.handleConfirm(CacheKey.NonSportsCacheKey, this.nonSportStatus);
        } else {
          e.source.checked = this.nonSportStatus;
        }
      });
  }

  createDialogMessage(status: boolean, cacheKey: string) {
    const contentStatus = status ? 'START' : 'STOP';
    const className = status ? 'blue' : 'red';
    const statusMessage = `<span class="${className}"><b>${contentStatus}</b></span>`;
    const serviceName =
      cacheKey === CacheKey.SportsCacheKey ? 'SportBook Sync' : 'NonSport Sync';

    const message = `Are you sure you want to ${statusMessage} ${serviceName} service for all SiteDB`;
    return message;
  }

  handleConfirm(cacheKey: string, status: boolean) {
    this.controlSyncService.updateStatus(status, cacheKey).subscribe((res) => {
      this.loadInfoService();
    });
  }
}
