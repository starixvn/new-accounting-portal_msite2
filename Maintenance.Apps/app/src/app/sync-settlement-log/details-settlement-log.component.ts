import { Component, Inject, OnInit, ViewEncapsulation } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { SettlementErrorLog } from './sync-settlement-log.model';

@Component({
    selector: 'app-details-settlement-log',
    templateUrl: './details-settlement-log.component.html',
    styleUrls: ['./details-settlement-log.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class DetailsSettlementLogComponent implements OnInit {

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: SettlementErrorLog,
    ) {

    }

    ngOnInit(): void {
        throw new Error("Method not implemented.");
    }
}