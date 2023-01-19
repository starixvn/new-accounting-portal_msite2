import { Component, Inject, OnInit, ViewEncapsulation } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { DetailSettlementErrorLog, SettlementErrorLog } from './sync-settlement-log.model';

@Component({
    selector: 'app-details-settlement-log',
    templateUrl: './details-settlement-log.component.html',
    styleUrls: ['./details-settlement-log.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class DetailsSettlementLogComponent implements OnInit {

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: {
            sessionId: string;
            logs: DetailSettlementErrorLog[]
        },
    ) {

    }

    ngOnInit(): void {
    }

    exportFile() {
        const value = this.data.logs.map(v => `LogId:${v.id}\n${v.transIds}`);
        const data = value.join('\n');

        this.saveTextAsFile(data, this.data.sessionId)
    }

    saveTextAsFile(data: string, filename: string) {
        const blob = new Blob([data], { type: 'text/plain;charset=utf-8', endings: 'native' });
        const a = document.createElement('a');

        a.download = filename;
        a.href = window.URL.createObjectURL(blob);
        a.click();
    }
}