import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { LogStatus } from './update-log-status.model';
import { UpdateLogStatusService } from './update-log-status.service';

@Component({
    selector: 'app-update-log-status-detail',
    templateUrl: './update-log-status-detail.component.html',
    styleUrls: ['./update-log-status-detail.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class UpdateLogStatusDetailComponent implements OnInit {
    constructor(
        private updateLogStatusService: UpdateLogStatusService,
        @Inject(MAT_DIALOG_DATA) public data: LogStatus
    ) { }

    ngOnInit(): void { }

    openUpdateConfirmDialog() {
        // this.updateLogStatusService.openUpdateConfirmDialog(this.data);
    }

    openDeleteConfirmDialog() {
        // this.updateLogStatusService.openDeleteConfirmDialog(this.data);
    }

    transformTransInfo(transInfo: string) {
        const parser = new DOMParser();
        const doc = parser.parseFromString(transInfo, 'application/xml');
        const rTag = doc.getElementsByTagName('r');
        return Array.from(rTag)
            .map((e) => e.getAttribute('t'))
            .join(',');
    }
}
