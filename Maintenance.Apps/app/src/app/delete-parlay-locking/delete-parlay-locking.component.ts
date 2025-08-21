import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { MatchParlayLockingService } from './delete-parlay-locking.service';
import {
    ParlayLockingRequest,
    MatchParlayLocking,
} from './match-parlay-locking.model';

// Time range constants in hours
const ONE_HOUR = 1;
const TWELVE_HOURS = 12;
const TWENTY_FOUR_HOURS = 24;

@Component({
    selector: 'app-delete-parlay-locking',
    templateUrl: './delete-parlay-locking.component.html',
    styleUrls: ['./delete-parlay-locking.component.scss'],
})
export class DeleteParlayLockingComponent implements OnInit {
    public filter: ParlayLockingRequest = { timeRange: ONE_HOUR };
    public timeRanges: number[] = [ONE_HOUR, TWELVE_HOURS, TWENTY_FOUR_HOURS];
    public matches: MatchParlayLocking[] = [];

    constructor(
        private readonly matchService: MatchParlayLockingService,
        private readonly dialogService: ConfirmDialogService
    ) { }

    ngOnInit(): void {
        this.loadData();
    }

    submit() {
        this.loadData();
    }

    loadData() {
        this.matchService.getAll(this.filter).subscribe((data) => {
            this.matches = data;
        });
    }

    openDeleteConfirmDialog(item: MatchParlayLocking) {
        const message = `
      <div>DB Server: ${item.serverName}</div>
      <div>MatchId:  ${item.matchId} </div>
      <div>Total Affected Refno(s): ${item.totalAffectedRefno} </div>
      <div>Session: ${item.session}</div>
      <div>Are you sure you want to <span class="red">DELETE</span> this row?
    `;
        const confirmStyle = 'btn-confirm-delete';

        this.dialogService
            .openConfirm({ message, confirmStyle })
            .afterClosed()
            .subscribe((res: boolean) => {
                res && this.handleDeleteItem(item);
            });
    }

    handleDeleteItem(item: MatchParlayLocking) {
        this.matchService.deleteItem(item).subscribe((res) => {
            this.loadData();
        });
    }
}
