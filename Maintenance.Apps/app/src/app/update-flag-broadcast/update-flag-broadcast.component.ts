import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { FlagBroadcast } from './update-flag-broadcast.model';
import { UpdateFlagBroadcastService } from './update-flag-broadcast.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './update-flag-broadcast.component.html',
  styleUrls: ['./update-flag-broadcast.component.scss'],
})
export class UpdateFlagBroadcastComponent implements OnInit {
  private readonly cockFighting = 11;
  private readonly virtualKiron = 15;
  public flagBroadcast!: FlagBroadcast;
  public flagStatus = false;
  public products: { id: number, name: string, isSelected: boolean }[] = [];
  public selectedProduct: number[] = [];

  constructor(
    private readonly dialogService: ConfirmDialogService,
    private readonly updateFlagBroadcastService: UpdateFlagBroadcastService
  ) { }

  ngOnInit(): void {
    this.products = [
      { id: 1, name: 'Bitcoin', isSelected: false },
      { id: 2, name: 'Happy5', isSelected: false },
      { id: 3, name: 'Number Game', isSelected: false },
      { id: 4, name: 'Outright', isSelected: false },
      { id: 5, name: 'Virtual Bet Rardar', isSelected: false },
      { id: 6, name: 'Virtual sports', isSelected: false },
      { id: 7, name: 'Bet Builder', isSelected: false },
      { id: 8, name: 'PinGoal', isSelected: false },
      { id: 9, name: 'Fast Market', isSelected: false },
      { id: 10, name: 'Leap Gaming - Cock-Fighting', isSelected: false },
      { id: 12, name: 'Virtual Sport 5', isSelected: false },
      { id: 13, name: 'Virtual Sport 6', isSelected: false },
      {
        id: 14,
        name: 'Virtual Racing 2 - Virtual Sports 7(Kiron)',
        isSelected: false,
      },
    ];
    this.loadInfo();
  }

  loadInfo() {
    this.updateFlagBroadcastService.getInfo().subscribe((res) => {
      this.flagBroadcast = res;
      this.flagStatus = this.flagBroadcast.currentStatus;
      const productType = this.flagBroadcast.productType.split(',');

      this.products.forEach((item: { id: number, name: string, isSelected: boolean }) => {
        item.isSelected = _.includes(productType, item.id.toString());
      });
    });
  }

  openConfirmDialog(e: { checked: boolean, source: { checked: boolean } }) {
    const message = this.createDialogMessage(e.checked);
    const confirmText = e.checked ? 'ON' : 'OFF';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';

    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.flagStatus = e.checked;
          this.handleConfirm(this.flagStatus, this.selectedProduct, e);
        } else {
          e.source.checked = this.flagStatus;
        }
      });
  }

  createDialogMessage(status: boolean) {
    const contentStatus = status ? 'ON' : 'OFF';
    const className = status ? 'blue' : 'red';
    const statusMessage = `<span class="${className}"><b>${contentStatus}</b></span>`;
    const message = `Are you sure you want to ${statusMessage} flag for deploying on Broadcast API?`;
    return message;
  }

  handleConfirm(status: boolean, productType: number[], e: { checked: boolean, source: { checked: boolean } }) {
    if (this.flagStatus && this.selectedProduct.length === 0) {
      alert('Please select product to ON.');
      this.flagStatus = false;
      e.source.checked = false;
    } else {
      this.updateFlagBroadcastService
        .updateFlag(status, productType)
        .subscribe((res) => {
          this.loadInfo();
        });
    }
  }

  openConfirmDialogStatusUm(e: { checked: boolean, source: { checked: boolean } }) {
    const message = this.createDialogMessageUm(e.checked);
    const confirmText = e.checked ? 'ON' : 'OFF';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';
    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          const status = e.checked;
          this.handleConfirmUm(status);
        } else {
          e.source.checked = this.flagBroadcast.settleUnderMaintenanceStatus;
        }
      });
  }

  handleConfirmUm(status: boolean) {
    this.updateFlagBroadcastService.updateFlagUm(status).subscribe((res) => {
      this.flagBroadcast.settleUnderMaintenanceStatus = res;
    });
  }

  createDialogMessageUm(status: boolean) {
    const contentStatus = status ? 'ON' : 'OFF';
    const className = status ? 'blue' : 'red';
    const statusMessage = `<span class="${className}"><b>${contentStatus}</b></span>`;
    const message = `Are you sure you want to ${statusMessage} flag settle on UM for Broadcast service?`;
    return message;
  }

  getCheckedItemList() {
    this.selectedProduct = [];
    const vsCockFighting = 10;
    const vsKiron = 14;
    for (const product of this.products) {
      if (product.isSelected) {
        const productsId = product.id;
        if (productsId === vsCockFighting) {
          this.selectedProduct.push(productsId);
          this.selectedProduct.push(this.cockFighting);
        } else if (productsId === vsKiron) {
          this.selectedProduct.push(productsId);
          this.selectedProduct.push(this.virtualKiron);
        } else {
          this.selectedProduct.push(product.id);
        }
      }
    }
  }
}
