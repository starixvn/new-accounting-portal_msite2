import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { SnackbarService } from '../shared/components/snackbar/snackbar.service';
import { SystemSettings } from './system-settings.model';
import { UpdateSystemSettingService } from './update-system-settings.service';

@Component({
  selector: 'app-update-system-settings',
  templateUrl: './update-system-settings.component.html',
  styleUrls: ['./update-system-settings.component.scss'],
})
export class UpdateSystemSettingsComponent implements OnInit {
  public systemSettings: SystemSettings[] | null = null;

  constructor(
    private service: UpdateSystemSettingService,
    private dialog: MatDialog,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.service.getAll().subscribe((data) => {
      console.log('get-system-setting', data);
      this.systemSettings = data;
    });
  }

  openUpdateConfirmDialog(item: SystemSettings) {
    const message = `
        <div>Info:<div>
        <div>DB Server: ${item.dbServer}<div>
        <div>VID: ${item.vId}<div>
        <div> Vname: ${item.vName}<div>
        <div>Vvalue: ${item.vValue}<div>
        <div>Are you sure you want to <b>UPDATE</b> this row!<div>
    `;
    const confirmText = 'Update';
    const cancelText = 'Cancel';

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message, confirmText, cancelText },
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      dialogResult && this.handleUpdate(item.dbServer);
    });
  }
  handleUpdate(dbServer: string) {
    this.service.update(dbServer).subscribe((res) => {
      this.snackbarService.openSnackBar();
    });
  }
}
